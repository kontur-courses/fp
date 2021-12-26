using System;
using System.Collections.Generic;
using System.IO;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class TxtFileReader : IFileReader
    {
        public Result<List<string>> GetWordsFromFile(string path, char[] separators)
        {
            try
            {
                return Result.Ok(ReadAllWordsFromFile(path, separators));
            }
            catch (Exception e)
            {
                return Result.Fail<List<string>>(e.Message);
            }
        }

        private static List<string> ReadAllWordsFromFile(string path, char[] separators)
        {
            using var streamReader = new StreamReader(path);
            var line = streamReader.ReadLine();
            var wordsList = new List<string>();

            while (!string.IsNullOrEmpty(line))
            {
                wordsList.AddRange(line.Split(separators, StringSplitOptions.RemoveEmptyEntries));
                line = streamReader.ReadLine();
            }

            return wordsList;
        }
    }
}