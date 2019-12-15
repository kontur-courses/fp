using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ResultOf;

namespace TagCloud
{
    public class WordsHandler : IWordsHandler
    {
        public Result<Dictionary<string, int>> Conversion(Dictionary<string, int> wordsAndCount)
        {
            var path = $"{GetCurrentDirectoryPath()}\\BoringWords.txt";
            return Result.Of( ()=>File.ReadAllLines(path)
                .Select(s => s.ToLower().Trim())
                .ToHashSet())
                .Then(bw => wordsAndCount
                    .Select(p => new KeyValuePair<string, int>(p.Key.ToLower(), p.Value))
                    .Where(p => !bw.Contains(p.Key))
                    .ToDictionary(p => p.Key, p => p.Value));

            //return wordsAndCount
            //    .Select(p => new KeyValuePair<string, int>(p.Key.ToLower(), p.Value))
            //    .Where(p => !boringWords.Contains(p.Key))
            //    .ToDictionary(p => p.Key, p => p.Value);
        }

        public Result<Dictionary<string, int>> GetWordsAndCount(string path)
        {
            return Result.Of(() =>File.ReadAllLines(path, Encoding.UTF8)
                .Where(s => s != string.Empty)
                .Select(s => s.Trim())
                .GroupBy(word => word.ToLower())
                .ToDictionary(g => g.Key, g => g.Count()))
                .RefineError($"Файла {path} не сущесвует");
        }

        private string GetCurrentDirectoryPath()
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        }
    }
}