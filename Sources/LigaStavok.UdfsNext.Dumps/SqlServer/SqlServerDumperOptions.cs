namespace LigaStavok.UdfsNext.Dumps.SqlServer
{
	public  class SqlServerDumperOptions
	{
		public int MaxDegreeOfParallelism { get; set; } = 100;

		public string ConnectionString { get; set; }

		public int BatchSize { get; set; } = 50;
		
		public string ServiceId { get; set; }

		public string DestinationTableName { get; set; } = "Dumps";
	}
}