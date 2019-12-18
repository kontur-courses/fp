using System.Collections.Generic;
using System.Linq;

namespace TagsCloudGenerator.WordsHandler.Filters
{
    public class BoringWordsEjector : IWordsFilter
    {
        private readonly HashSet<string> boringWords;

        public BoringWordsEjector(IEnumerable<string> boringWords)
        {
            this.boringWords = new HashSet<string>(boringWords);
        }

        public Result<Dictionary<string, int>> Filter(Dictionary<string, int> wordToCount)
        {
            return Result
                .Of(() => FilterBoringWord(wordToCount))
                .RefineError("Error while filtering");
        }

        private Dictionary<string, int> FilterBoringWord(Dictionary<string, int> wordToCount)
        {
            return wordToCount
                .Where(pair => !boringWords.Contains(pair.Key))
                .ToDictionary(x => x.Key, x => x.Value);
        }

//        private Result<HashSet<string>> GetBoringWords()
//        {
//            if (string.IsNullOrEmpty(boringWordsFilePath))
//                return new HashSet<string>().AsResult();

//            return Result
//                .Of(() => PathHelper.GetFileExtension(boringWordsFilePath))
//                .Then(extension => readerFactory.GetReaderFor(extension))
//                .Then(reader => reader.ReadWords(boringWordsFilePath))
//                .Then(words => new HashSet<string>(words.Keys))
//                .RefineError($"couldn't get boring words from '{boringWordsFilePath}'");
//        }
    }
}