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
            return lines.Any(line => (line).Contains(' '))
                ? Result.Fail<List<string>>("Each line must contain only one word")
                : Result.Ok(lines.Where(line => !string.IsNullOrEmpty(line)).ToList());
        }
    }
}