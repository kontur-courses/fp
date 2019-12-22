using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ResultOf;

namespace TagCloud
{
    public class WordsHandler : IWordsHandler
    {
        public Result<Dictionary<string, int>> Conversion(Dictionary<string, int> wordsAndCount)
        {
            return Result.Of(() => $"{GetCurrentDirectoryPath()}\\BoringWords.txt")
                    .Then(pathToBoringWords => File.ReadAllLines(pathToBoringWords)
                        .Select(s => s.ToLower().Trim())
                        .ToHashSet())
                    .Then(bw => wordsAndCount
                        .Select(p => new KeyValuePair<string, int>(p.Key.ToLower(), p.Value))
                        .Where(p => !bw.Contains(p.Key))
                        .ToDictionary(p => p.Key, p => p.Value));
        }

        public Result<Dictionary<string, int>> GetWordsAndCount(string path)
        {
            return Result.Of(() => File.ReadAllLines(path, Encoding.UTF8)
                    .SelectMany(s => Regex.Split(s.ToLower().Trim(), @"\W|_", RegexOptions.IgnoreCase))
                    .Where(s => s != string.Empty)
                    .GroupBy(word => word)
                    .ToDictionary(g => g.Key, g => g.Count()))
                    .ReplaceError(error => 
                        error.Replace("Значение не может быть неопределенным.\r\n","Параметр не определен"));
        }

        private string GetCurrentDirectoryPath()
        {
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent;
            if (directoryInfo != null)
                return directoryInfo.FullName;
            throw new ArgumentException("Файл со скучными словами не найден");
        }
    }
}