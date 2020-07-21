using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LigaStavok.UdfsNext.Dumps.SqlServer
{
	public class SqlServerDumper : IMessageDumper
	{
		private readonly ILogger logger;
		private readonly SqlServerDumperOptions options;
		private readonly ActionBlock<IEnumerable<MessageContext<DumpMessage>>> writeBlock;
		private readonly BatchBlock<MessageContext<DumpMessage>> bufferBlock;


		public SqlServerDumper(
			ILogger<SqlServerDumper> logger,
			IOptions<SqlServerDumperOptions> options
		) :this(logger, options.Value)
		{ }

		internal SqlServerDumper(
			ILogger logger,
			SqlServerDumperOptions options
		)
		{
			this.options = options;
			this.logger = logger;

			bufferBlock = new BatchBlock<MessageContext<DumpMessage>>(options.BatchSize);

			writeBlock = new ActionBlock<IEnumerable<MessageContext<DumpMessage>>>(
				WriteMessage,
				new ExecutionDataflowBlockOptions()
				{
					MaxDegreeOfParallelism = options.MaxDegreeOfParallelism
				}
			);

			bufferBlock.LinkTo(writeBlock);
		}

		private async Task WriteMessage(IEnumerable<MessageContext<DumpMessage>> messages)
		{
			try
			{
				using (var dt = new DataTable())
				{

					dt.Columns.Add("RowId", typeof(long));
					dt.Columns.Add("ServiceId", typeof(string));
					dt.Columns.Add("Source", typeof(string));
					dt.Columns.Add("MessageType", typeof(string));
					dt.Columns.Add("EventId", typeof(string));
					dt.Columns.Add("DateReceived", typeof(DateTimeOffset));
					dt.Columns.Add("IncomingId", typeof(string));
					dt.Columns.Add("Body", typeof(string));

					foreach (var item in messages)
					{
						var dr = dt.NewRow();

						dr["ServiceId"] = options.ServiceId;
						dr["Source"] = item.Message.Source;
						dr["MessageType"] = item.Message.MessageType;
						dr["DateReceived"] = item.ReceivedOn;
						dr["IncomingId"] = item.IncomingId;
						dr["EventId"] = item.Message.EventId;
						dr["Body"] = item.Message.MessageBody;

						dt.Rows.Add(dr);
					}

					using (var sqlBulkCopy = new SqlBulkCopy(options.ConnectionString))
					{
						sqlBulkCopy.DestinationTableName = options.DestinationTableName;
						await sqlBulkCopy.WriteToServerAsync(dt);
					}
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Writing sql server dump error.");
			}
		}

		public void Write(MessageContext<DumpMessage> dumpMessage)
		{
			bufferBlock.Post(dumpMessage);
		}
	}
}
