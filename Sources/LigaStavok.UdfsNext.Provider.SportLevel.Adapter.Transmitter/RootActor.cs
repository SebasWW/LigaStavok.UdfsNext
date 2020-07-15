using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Udfs.Common;
using Udfs.Common.Actor;
using Udfs.Transmitter.Plugin;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class RootActor : ReceiveActor, IPluginRootActor
	{
		private readonly ICanTell _transmitterActor;

		public RootActor(

			ActorMetadata<TransmitterRootActor> transmitterActorMeta
		)
		{

			_transmitterActor = Context.ActorSelection(transmitterActorMeta.Path);
		}
	}
}
