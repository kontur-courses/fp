using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using App.Infrastructure.FileInteractions.Readers;

namespace App.Implementation.FileInteractions.Readers
{
    public class FromStreamReader : ILinesReader
    {
        private readonly string fileName;

        public FromStreamReader(string fileName)
        {
            this.fileName = fileName;
        }

        public Result<IEnumerable<string>> ReadLines()
        {
            return ReadFromStream()
                    .Then(words => words
                        .SelectMany(line => Regex
                            .Split(line, @"\P{L}+", RegexOptions.Compiled))
                        .Select(word => word))
                .RefineError("Can not read lines from stream");
        }

        private Result<IEnumerable<string>> ReadFromStream()
        {
            if (!File.Exists(fileName))
                return Result.Fail<IEnumerable<string>>($"File {fileName} is not found");

            using var streamReader = new StreamReader(fileName, Encoding.UTF8);

            var words = new List<string>();
            var line = streamReader.ReadLine();

            while (!string.IsNullOrWhiteSpace(line))
            {
                words.Add(line);
                line = streamReader.ReadLine();
            }

            return words;
        }
    }
}