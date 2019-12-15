using System.Collections.Generic;
using TagsCloud.Interfaces;

namespace TagsCloud
{
	public class BoringWordsFilter: IWordFilter
	{
		private readonly ITextReader textReader;
		private HashSet<string> boringWords;
		
		public BoringWordsFilter(ITextReader textReader) => this.textReader = textReader;

		public Result<bool> CheckWord(string word) =>
			Result.Ok()
				.Then(_ => ReadWords())
				.Then(_ => !boringWords.Contains(word));

		private Result<HashSet<string>> ReadWords() =>
			boringWords != null
				? Result.Ok<HashSet<string>>(null)
				: textReader.Read().Then(words => boringWords = new HashSet<string>(words));
	}
}