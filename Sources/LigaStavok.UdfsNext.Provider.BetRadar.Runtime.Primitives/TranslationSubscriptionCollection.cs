﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LigaStavok.UdfsNext.Provider.BetRadar
{
	public class TranslationSubscriptionCollection : ConcurrentDictionary<long, TranslationSubscription>
	{
	}
}
