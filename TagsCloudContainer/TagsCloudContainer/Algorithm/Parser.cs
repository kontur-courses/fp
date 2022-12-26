using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Algorithm
{
    public class Parser : IParser
    {
        public Result<Dictionary<string, int>> CountWordsInFile(string pathToFile)
        {
            if (!File.Exists(pathToFile))
                return Result.Fail<Dictionary<string, int>>($"Файла не существует\t{pathToFile}");

            var wordsCount = new Dictionary<string, int>();
            using var reader = new StreamReader(pathToFile);
            while (reader.ReadLine()?.Trim().ToLower() is { } line)
            {
                if (line.Any(char.IsWhiteSpace))
                    return Result.Fail<Dictionary<string, int>>("Файл некорректен (содержит пробелы в словах)");
                wordsCount[line] = wordsCount.ContainsKey(line) ? wordsCount[line] + 1 : 1;
            }

            return Result.Ok(wordsCount);
        }

        public Result<HashSet<string>> FindWordsInFile(string pathToFile)
        {
            if (!File.Exists(pathToFile))
                return Result.Fail<HashSet<string>>($"Файла не существует\t{pathToFile}");

            var words = new HashSet<string>();
            using var reader = new StreamReader(pathToFile);
            while (reader.ReadLine()?.Trim().ToLower() is { } line)
            {
                if (line.Any(char.IsWhiteSpace))
                    return Result.Fail<HashSet<string>>("Файл некорректен (содержит пробелы в словах)");
                words.Add(line);
            }

            return Result.Ok(words);
        }
    }
}
