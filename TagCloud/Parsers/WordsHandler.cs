using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ResultOf;
using TagCloud.Factories;

namespace TagCloud
{
    public class WordsHandler : IWordsHandler
    {
        private readonly IBoringWordsFactory boringWordsFactory;
        private readonly IFileReader fileReader;

        public WordsHandler(IBoringWordsFactory boringWordsFactory, IFileReader fileReader)
        {
            this.boringWordsFactory = boringWordsFactory;
            this.fileReader = fileReader;
        }
        public Result<Dictionary<string, int>> RemoveBoringWords(Dictionary<string, int> wordsAndCount,
                string pathToBoringWords)
        {
            return boringWordsFactory.GetFromFile(pathToBoringWords)
                .Then(bw => wordsAndCount
                    .Select(p => new KeyValuePair<string, int>(p.Key.ToLower(), p.Value))
                    .Where(p => !bw.Contains(p.Key))
                    .ToDictionary(p => p.Key, p => p.Value));
        }

        public Result<Dictionary<string, int>> GetWordsAndCount(string path)
        {
            return fileReader.ReadWordsFromFile(path)
                    .Then(words => words
                    .GroupBy(word => word)
                    .ToDictionary(g => g.Key, g => g.Count()))
                    .ReplaceError(error =>
                        error.Replace("Значение не может быть неопределенным.\r\n", "Параметр не определен"));
        }
    }
}