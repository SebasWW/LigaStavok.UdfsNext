using System;

namespace Udfs.BetradarUnifiedFeed.Plugin.Abstractions
{
    public interface IFeedMessageParsingResult 
    {
        DumpMeta GetDumpMeta();
    }
}