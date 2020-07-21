namespace LigaStavok.UdfsNext.Configuration
{
	public class SqlServerDumpConfiguration
	{
		public bool Enabled { get; set; }

		public int MaxDegreeOfParallelism { get; set; } = 100;

		public string ConnectionString { get; set; }

		public int BatchSize { get; set; } = 50;

		public string ServiceId { get; set; }

		public string TableName { get; set; } = "Dumps";
	}
}
