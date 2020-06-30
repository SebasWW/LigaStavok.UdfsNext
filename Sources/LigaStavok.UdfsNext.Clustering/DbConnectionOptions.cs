namespace LigaStavok.UdfsNext.Orleans
{
	public class DbConnectionOptions
	{
        /// <summary>
        /// Enabling flag.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The connection string to the storage.
        /// </summary>
        public string ConnectionString { get; set; }

        /// 
        /// ADO.NET Provider path.
        /// </summary>
        public string Provider { get; set; } 
    }
}