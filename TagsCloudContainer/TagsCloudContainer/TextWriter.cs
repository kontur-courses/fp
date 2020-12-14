using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ResultOf;
using TagsCloudContainer.TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagsCloudContainer
{
    public class TextWriter : ITextWriter
    {
        public void WriteText(string text, string savePath)
        {
            var newText = GetParsedText(text);

            var separator = Path.DirectorySeparatorChar;
            var pattern = $@"((?:[^\{separator}]*\{separator})*)(.*[.].+)";
            var match = Regex.Match(savePath, pattern);

            Result.Ok(match)
                .Then(ValidateMatch)
                .Then(ValidateDirectoryPath)
                .Then(ValidateFileName)
                .OnFail(e => throw new ArgumentException(e, nameof(savePath)))
                .Then(x => File.WriteAllText(savePath, newText));
        }

        private string GetParsedText(string text)
        {
            var matches = Regex.Matches(text, @"\b\w+\b");

            if (matches.Count == 0)
                return "";

            return matches
                .Select(x => x.Value)
                .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
        }

        private Result<Match> ValidateMatch(Match match)
        {
            return Validate(match, x => !match.Success, $"Wrong path format: {match.Value}");
        }

        private Result<Match> ValidateDirectoryPath(Match match)
        {
            var path = match.Groups[1].ToString();
            return Validate(match, x => !Directory.Exists(path), $"Directory {path} doesnt exists");
        }

        private Result<Match> ValidateFileName(Match match)
        {
            var fileName = match.Groups[2].ToString();
            return Validate(match, x => !match.Groups[2].Success, $"Wrong file name: {fileName}");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string exception)
        {
            return predicate(obj)
                ? Result.Fail<T>(exception)
                : Result.Ok(obj);
        }
    }
}