using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LigaStavok.UdfsNext.Provider.SportLevel.State
{
	public interface IStateManager
	{
		Task<TranslationState> GetTranslationStateAsync(long translationId);
		Task SaveTranslationStateAsync(long translationId);
	}
}
