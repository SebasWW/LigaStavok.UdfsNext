//using LigaStavok.UdfsNext.Provider.SportLevel.WebSocket.Messages.Data;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;
//using Udfs.Transmitter.Messages;
//using Xunit;

//namespace UdfsNext.Provider.SportLevel.Adapter.MarketEventAdapterTests
//{
//	public class MarketEventAdapterTest
//	{

//        [Fact]
//        public async Task Markets()
//        {
//            var jsonEvent = File.ReadAllText(".\\Adapters\\MarketEventAdapterTest.json");
//            var jsonConfig = File.ReadAllText(".\\appsettings.json");

//            var message = JsonConvert.DeserializeObject<EventData>(jsonEvent).ToImmutable();
//            var cfg = JsonConvert.DeserializeObject<ServiceConfiguration>(jsonConfig);
//            var adapterConfiguration = cfg.SportLevel.Adapter; 

//            var state = new MemoryStateManager();

//            var adapter = new MarketEventAdapter(state, adapterConfiguration);
//            var result = await adapter.AdaptAsync(new MessageContext<DataEvent>(message));

//            var duplicatedIds = result
//                .Select(t => t as CreateUpdateMarketsCommand)
//                .Where(
//                    t => t.Selections
//                        .GroupBy(s => s.SelectionId)
//                        .Any(s => s.Count() > 1)
//                );

//            Assert.Empty(duplicatedIds);
//        }
//    }
//}
