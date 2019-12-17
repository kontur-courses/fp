using System.Collections.Generic;
using System.Linq;
using TagsCloud.Interfaces;

namespace TagsCloud
{
	public class WordsProcessor: IWordsProcessor
	{
		private readonly ITextReader textReader;
		private readonly IEnumerable<IWordFilter> wordFilters;

		public WordsProcessor(ITextReader textReader, IEnumerable<IWordFilter> wordFilters)
		{
			this.textReader = textReader;
			this.wordFilters = wordFilters;
		}

		public Result<IEnumerable<Word>> GetWordsWithFrequencies()
		{
			return FilterWords()
				.Then(words => words
					.Aggregate(new Dictionary<string, int>(),
						(frequencies, word) =>
						{
							if (frequencies.ContainsKey(word)) frequencies[word]++;
							else frequencies[word] = 1;
							return frequencies;
						})
					.OrderByDescending(pair => pair.Value)
					.Select(pair => new Word(pair.Key, pair.Value)));
		}

		private Result<IEnumerable<string>> FilterWords()
		{
			return textReader.Read()
				.Then(words => words.Select(w => w.ToLower()))
				.Then(words => words
					.Where(word => wordFilters.All(filter => filter.CheckWord(word).GetValueOrThrow())));
		}
	}
}