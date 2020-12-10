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
            var matches = Regex.Matches(text, @"\b\w+\b");
            var newText = "";

            if (matches.Count != 0)
                newText = matches
                    .Select(x => x.Value)
                    .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");

            Result.Ok(savePath)
                .Then(PathInRightFormat)
                .OnFail(e => throw new ArgumentException(e))
                .Then(x => File.WriteAllText(savePath, newText));
        }

        private static Result<string> PathInRightFormat(string path)
        {
            var separator = Path.DirectorySeparatorChar;
            var pattern = $@"((?:[^\{separator}]*\{separator})*)(.*[.].+)";
            var match = Regex.Match(path, pattern);
            var directoryPath = match.Groups[1].ToString();

            return Directory.Exists(directoryPath) && match.Groups[2].Success
                ? Result.Ok(path)
                : Result.Fail<string>("wrong path format");
        }
    }
}