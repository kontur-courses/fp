using System;
using System.Drawing;
using TagsCloudContainer;

namespace TagsCloudApp.Parsers
{
    public class ArgbColorParser : IArgbColorParser
    {
        public Result<Color> TryParse(string value)
        {
            return Result.Of(() => ColorTranslator.FromHtml(value))
                .ReplaceError(_ => $"Incorrect color: {value}");
        }
    }
}