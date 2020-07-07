using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Orleans.Client
{
	public class UdfsClusterClientsLocator : IReadOnlyDictionary<string, UdfsClusterClient>
	{
		private readonly Dictionary<string, IUdfsClusterClient> clusterClients 
			= new Dictionary<string, IUdfsClusterClient>();

		internal void Add(string key, IUdfsClusterClient udfsClusterClient)
		{
			clusterClients.Add(key, udfsClusterClient);
		}


#region ReadOnlyDictionary
		public IUdfsClusterClient this[string key] => clusterClients[key];

		public IEnumerable<string> Keys => clusterClients.Keys;

		public IEnumerable<IUdfsClusterClient> Values => clusterClients.Values;

		public int Count => clusterClients.Count;

		public bool ContainsKey(string key) => clusterClients.ContainsKey(key);

		public IEnumerator<KeyValuePair<string, IUdfsClusterClient>> GetEnumerator() => clusterClients.GetEnumerator();

		public bool TryGetValue(string key, out IUdfsClusterClient value) => clusterClients.TryGetValue(key, out value);

		IEnumerator IEnumerable.GetEnumerator() => clusterClients.GetEnumerator();
#endregion

	}
}
