using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class TxtFileReader : IFileReader
    {
        public Result<IEnumerable<string>> GetWordsFromFile(string path, char[] separators)
        {
            if (!File.Exists(path))
                return Result.Fail<IEnumerable<string>>($"file {path} does not exist");

            try
            {
                using var reader = new StreamReader(path);
                return Result.Ok(ReadAllWordsFromReader(reader, separators));
            }
            catch (Exception e)
            {
                return Result.Fail<IEnumerable<string>>(e.Message);
            }
        }

        private static IEnumerable<string> ReadAllWordsFromReader(TextReader reader, char[] separators)
        {
            var line = reader.ReadLine();

            while (!string.IsNullOrEmpty(line))
            {
                foreach (var word in line.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                    yield return word;

                line = reader.ReadLine();
            }
        }
    }
}