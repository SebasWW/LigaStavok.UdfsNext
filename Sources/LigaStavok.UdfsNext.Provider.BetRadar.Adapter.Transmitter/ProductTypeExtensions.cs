﻿using System;
using Udfs.BetradarUnifiedFeed.Plugin.Abstractions;
using Udfs.Transmitter.Messages.Identifiers;

namespace Udfs.BetradarUnifiedFeed.Plugin.Adapter.Extensions
{
    public static class ProductTypeExtensions
    {
        public static LineService ToLineService(this ProductType product)
        {
            switch (product)
            {
                case ProductType.LiveOdds:
                    return LineService.BetradarUnifiedFeedLiveOdds;
                case ProductType.MTS:
                    return LineService.BetradarUnifiedFeedMTS;
                case ProductType.LCoO:
                    return LineService.BetradarUnifiedFeedLCoO;
                case ProductType.Virtuals:
                    return LineService.BetradarUnifiedFeedVirtuals;
                case ProductType.BetPal:
                    //TODO: Add BetPal LineService Id
                case ProductType.PremiumCricket:
                    //TODO: Add PremiumCricket LineService Id
                default:
                    throw new ArgumentOutOfRangeException(nameof(product), product, $"Product [{product}] is not supported.");
            }
        }
    }
}