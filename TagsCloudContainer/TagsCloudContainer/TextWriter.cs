using System;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloudContainer.TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagsCloudContainer
{
    public class TextWriter : ITextWriter
    {
        public void WriteText(string text, ITextSaver saver)
        {
            var parsedText = GetParsedText(text);

            saver.Save(parsedText);
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
    }
}