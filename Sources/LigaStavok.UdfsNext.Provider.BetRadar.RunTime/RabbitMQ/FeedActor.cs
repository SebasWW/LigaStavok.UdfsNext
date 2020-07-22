using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using NLog;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
using LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ.Messages;
using Udfs.BetradarUnifiedFeed.Plugin.InputDumping;
using Udfs.BetradarUnifiedFeed.Plugin.LastMessageTs;
using Udfs.BetradarUnifiedFeed.Plugin.Product;
using Udfs.BetradarUnifiedFeed.Plugin.Product.Messages;
using Udfs.BetradarUnifiedFeed.Plugin.Testing;
using Udfs.Common.Actor;
using Udfs.Common.Messages;

namespace LigaStavok.UdfsNext.Provider.BetRadar.RabbitMQ
{
    public sealed class FeedActor : BetradarUnifiedActorBase
    {
        private readonly IActorRef _lastMessageTs;
        private readonly IReadOnlyDictionary<ProductType, IActorRef> _products;
        private readonly IActorRef _feedClient;

        public FeedActor(
            ActorMetadata<ProductActor> productActorMetadata,
            ActorMetadata<LastMessageTsActor> lastMessageTsActorMetadata,
            ActorMetadata<InputMessageDumperActor> inputDumperActorMetadata,
            DumpsPlayerOptions dumpsPlayerOptions,
            FeedConfiguration feedConfiguration,
            LogFactory logFactory
        ) : base(logFactory)
        {
            _products = CreateProductsActors(productActorMetadata.ActorType);
            _lastMessageTs = Context.ActorOf(lastMessageTsActorMetadata);

            if (!dumpsPlayerOptions.IsEnabled)
            {
                _feedClient = Context.ActorOf(!feedConfiguration.AllowMessagesFromApi
                    ? Context.DI().Props<FeedClientActor>()
                    : Context.DI().Props<FeedWithApiMessageClientActor>());
            }

            Context.ActorSelection(inputDumperActorMetadata);

            Receive<EnableProduct>(
                msg => _products[msg.ProductType].Forward(msg));

            Receive<OddsRecoveryRequested>(
                msg => _products[msg.ProductType].Forward(msg));

            Receive<FeedMessageParsed>(msg =>
            {
                _products[msg.Message.Product].Forward(msg);
                _lastMessageTs.Forward(msg);
            });

            ReceiveAsync<ShutdownCommand>(Shutdown);
        }

        private static IReadOnlyDictionary<ProductType, IActorRef> CreateProductsActors(Type productActorType)
        {
            var products = new Dictionary<ProductType, IActorRef>();

            foreach (var productType in Enum.GetValues(typeof(ProductType)).Cast<ProductType>())
            {
                var productProps = Context.DI().Props(productActorType);
                var product = Context.ActorOf(productProps, productType.ToString());

                products[productType] = product;
            }

            return products;
        }

        private async Task Shutdown(ShutdownCommand message)
        {
            try
            {
                await _lastMessageTs.GracefulStop(TimeSpan.FromSeconds(10), message);
                await _feedClient.GracefulStop(TimeSpan.FromSeconds(10), message);
                await Task.WhenAll(_products.Select(x => x.Value.GracefulStop(TimeSpan.FromSeconds(10), message)));
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                Context.Stop(Self);
            }
        }
    }
}