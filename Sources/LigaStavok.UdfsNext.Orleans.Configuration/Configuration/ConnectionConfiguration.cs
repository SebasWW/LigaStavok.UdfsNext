namespace LigaStavok.UdfsNext.Configuration
{
	public class ConnectionConfiguration
	{
        /// <summary>
        /// Enabling flag.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The connection string to the storage.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary> 
        /// ADO.NET Provider path.
        /// </summary>
        public string Provider { get; set; } = "System.Data.SqlClient";


  //      /// <summary> 
  //      /// Storage name.
  //      /// </summary>
		//public string Name { get;  set; }
	}
}