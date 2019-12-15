using System.Collections.Generic;

namespace TagsCloud.Interfaces
{
	public interface IWordsProcessor
	{
		Result<IEnumerable<Word>> GetWordsWithFrequencies();
	}
}