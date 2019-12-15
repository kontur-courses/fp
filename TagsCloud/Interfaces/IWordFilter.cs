using System.Collections.Generic;

namespace TagsCloud.Interfaces
{
	public interface IWordFilter
	{
		Result<bool> CheckWord(string word);
	}
}