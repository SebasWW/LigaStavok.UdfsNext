namespace LigaStavok.UdfsNext.Orleans.Grains
{
	public interface IRecoverableState
	{
		bool Saved { get; set; }
	}
}
