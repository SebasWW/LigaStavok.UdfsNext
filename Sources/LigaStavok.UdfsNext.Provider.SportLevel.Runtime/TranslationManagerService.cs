using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LigaStavok.UdfsNext.Provider.SportLevel
{
	public class TranslationManagerService : BackgroundService
	{
		private readonly ITranslationManager translationManager;

		public TranslationManagerService(
			ITranslationManager translationManager
		)
		{
			this.translationManager = translationManager;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			return translationManager.StartAsync(cancellationToken);
		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{
			return translationManager.StopAsync(cancellationToken);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			return translationManager.ExecuteAsync(stoppingToken);
		}
	}
}
