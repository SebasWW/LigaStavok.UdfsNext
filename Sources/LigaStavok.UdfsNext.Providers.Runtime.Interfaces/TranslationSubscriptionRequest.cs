namespace LigaStavok.UdfsNext.Providers
{
	public class TranslationSubscriptionRequest<TState>
	{
		public long Id { get; set; }

		public TState State { get; set; }
	}
}
