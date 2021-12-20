using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.TextPreparation
{
    public class WordsReader : IWordsReader
    {
        public Result<List<string>> ReadAllWords(string fileContent)
        {
            var lines = fileContent.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            if (lines.Any(line => (line).Contains(' ')))
            {
                return Result.Fail<List<string>>("Each line must contain only one word");
            }

            return Result.Ok(lines.Where(line => !string.IsNullOrEmpty(line)).ToList());
        }
    }
}