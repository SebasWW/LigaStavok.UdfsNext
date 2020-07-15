using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Dumps.FileSystem
{
	public class FileDumper : IMessageDumper
	{
		private readonly FileDumperOptions options;
		private readonly ActionBlock<MessageContext<DumpMessage>> writeBlock;
		private readonly ILogger<FileDumper> logger;

		public FileDumper(
			ILogger<FileDumper> logger,
			IOptions<FileDumperOptions> options
		)
		{
			this.options = options.Value;
			this.logger = logger;

			writeBlock = new ActionBlock<MessageContext<DumpMessage>>(
				WriteMessage,
				new ExecutionDataflowBlockOptions()
				{
					MaxDegreeOfParallelism = options.Value.MaxDegreeOfParallelism
				}
			);
		}

		private Task WriteMessage(MessageContext<DumpMessage> messageContext)
		{
			try
			{
				var message = messageContext.Message;

				var dumpDir = Path.Combine(options.RootDirectory, message.SourceType, message.EventId);
				Directory.CreateDirectory(dumpDir);

				var dumpPath = Path.Combine(dumpDir,
					$"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss_fff}__{messageContext.IncomingId}__{message.MessageType}.txt");

				return File.AppendAllTextAsync(dumpPath, message.MessageBody);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Writing dump error.");
				return Task.CompletedTask;
			}
		}

		public void Write(MessageContext<DumpMessage> dumpMessage)
		{
			writeBlock.Post(dumpMessage);
		}
	}
}
