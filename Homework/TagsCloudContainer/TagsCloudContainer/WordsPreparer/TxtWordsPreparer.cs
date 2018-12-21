using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudResult;

namespace TagsCloudBuilder
{
    public class TxtWordsPreparer : IWordsPreparer
    {
        private readonly string fileName;

        public TxtWordsPreparer(string fileName)
        {
            this.fileName = fileName;
        }

        public Result<Dictionary<string, int>> GetPreparedWords()
        {
            var wordsWithFrequency = new Dictionary<string, int>();
            if (!File.Exists(fileName))
                return Result.Fail<Dictionary<string, int>>(
                    $"Something went wrong. Check the correctness of {fileName} path.");

            foreach (var word in File.ReadAllLines(fileName).Where(word => word.Length > 0))
            {
                var lowerWord = word.ToLower();
                if (!wordsWithFrequency.ContainsKey(lowerWord))
                    wordsWithFrequency.Add(lowerWord, 1);
                else
                    wordsWithFrequency[lowerWord] += 1;
            }

            return wordsWithFrequency;
        }

        public static Result<Dictionary<string, int>> ReadAllLines(string fileName)
        {
            var wordsWithFrequency = new Dictionary<string, int>();

            if (!File.Exists(fileName))
                return Result.Fail<Dictionary<string, int>>(
                    $"Something went wrong. Check the correctness of {fileName} path.");

            foreach (var word in File.ReadAllLines(fileName))
            {
                var lowerWord = word.ToLower();
                if (!wordsWithFrequency.ContainsKey(lowerWord))
                    wordsWithFrequency.Add(lowerWord, 1);
                else
                    wordsWithFrequency[lowerWord] += 1;
            }

            return wordsWithFrequency;
        }
    }
}
