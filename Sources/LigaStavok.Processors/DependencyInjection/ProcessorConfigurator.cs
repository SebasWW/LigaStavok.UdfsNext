namespace LigaStavok.Processors.DependencyInjection
{
	public class ProcessorConfigurator
	{
		private bool reentrancable = false;
		public ProcessorConfigurator SetReentrancable()
		{
			reentrancable = true;
			return this;
		}

		private int threadCount = 1;
		public ProcessorConfigurator SetThreadCount(int count)
		{
			if (count < 1)
			{
				throw new System.ArgumentNullException(nameof(count));
			}

			threadCount = count;
			return this;
		}

		public void Configure(Processor processor)
		{
			processor.Options.Reentrancable = reentrancable;
			processor.Options.ThreadCount = threadCount;
		}
	}
}