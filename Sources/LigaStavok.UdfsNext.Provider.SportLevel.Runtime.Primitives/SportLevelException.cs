using System;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	[Serializable]
	public class SportLevelException : Exception
	{
		public SportLevelException() { }
		public SportLevelException(string message) : base(message) { }
		public SportLevelException(string message, Exception inner) : base(message, inner) { }
		protected SportLevelException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
