using System.Collections.Generic;
using NLog;
using Udfs.Transmitter.Messages.Interfaces;

namespace Udfs.BetradarUnifiedFeed.Plugin.Abstractions
{
    public interface ITransmitterCommandsFactory
    {
        IEnumerable<ITransmitterCommand> CreateTransmitterCommands(AdaptMessage message, Logger logger);
    }
}