using System;
using System.Collections.Generic;
using System.IO;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class TxtFileReader : IFileReader
    {
        public Result<IEnumerable<string>> GetWordsFromFile(string path, char[] separators)
        {
            try
            {
                return Result.Ok(ReadAllWordsFromReader(path, separators));
            }
            catch (Exception e)
            {
                return Result.Fail<IEnumerable<string>>(e.Message);
            }
        }

        private static IEnumerable<string> ReadAllWordsFromReader(string path, char[] separators)
        {
            using var reader = new StreamReader(path);
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