using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using ResultOf;
using Xceed.Words.NET;

namespace TagsCloudContainer.Reading

{
    public class DocWordsReader : IWordsReader
    {
        public Result<ReadOnlyCollection<string>> ReadWords(string inputPath)
        {
            return ReadFile(inputPath).Then(ParseWords).RefineError("Error reading words");
        }

        private Result<string> ReadFile(string inputPath)
        {
            return Result.Of(() => DocX.Load(inputPath).Text, $"Error reading file {inputPath}");
        }

        private ReadOnlyCollection<string> ParseWords(string text)
        {
            var regex = new Regex("\\W?(\\w+)\\W");
            var matches = regex.Matches(text);
            var res = new List<string>();
            foreach (Match match in matches)
            {
                res.Add(match.Groups[1].Value);
            }

            return new ReadOnlyCollection<string>(res);
        }
    }
}