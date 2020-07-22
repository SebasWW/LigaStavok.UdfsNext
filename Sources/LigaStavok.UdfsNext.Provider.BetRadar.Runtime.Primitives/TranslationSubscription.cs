using System;
using System.Collections.Generic;
using System.Text;
using LigaStavok.UdfsNext.Provider.BetRadar.State;

namespace LigaStavok.UdfsNext.Provider.BetRadar
{
	public class TranslationSubscription
	{
		private readonly Action saveStateAction;

		public TranslationSubscription(Action saveStateAction)
		{
			this.saveStateAction = saveStateAction;
		}

		public TranslationState PersistableState { get; set; }

		public int MetaHash { get; set; }

		public void SaveState()
		{
			saveStateAction?.Invoke();
		}
	}
}
