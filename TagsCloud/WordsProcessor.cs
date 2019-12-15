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
			var frequencies = new Dictionary<string, int>();
			var filteredWords = FilterWords();
			if (!filteredWords.IsSuccess)
				return Result.Fail<IEnumerable<Word>>(filteredWords.Error);
			foreach (var word in filteredWords.Value)
			{
				if (frequencies.ContainsKey(word))
					frequencies[word]++;
				else
					frequencies[word] = 1;
			}

			var resultWords = frequencies
				.OrderByDescending(pair => pair.Value)
				.Select(pair => new Word(pair.Key, pair.Value));
			return Result.Ok(resultWords);
		}

		private Result<IEnumerable<string>> FilterWords()
		{
			var words = textReader.Read();
			if (!words.IsSuccess)
				return Result.Fail<IEnumerable<string>>(words.Error);
			var resultWords = new List<string>();
			
			foreach (var word in words.Value.Select(w => w.ToLower()))
			{
				var results = wordFilters.Select(filter => filter.CheckWord(word));
				var wordIsCorrect = true;
				foreach (var result in results)
				{
					if (!result.IsSuccess)
						return Result.Fail<IEnumerable<string>>(result.Error);
					if (result.Value) continue;
					wordIsCorrect = false;
					break;
				}
				if (wordIsCorrect)
					resultWords.Add(word);
			}
			return resultWords;
		}
	}
}