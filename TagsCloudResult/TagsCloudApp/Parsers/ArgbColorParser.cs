using System.Drawing;
using TagsCloudContainer.Results;

namespace TagsCloudApp.Parsers
{
    public class ArgbColorParser : IArgbColorParser
    {
        public Result<Color> Parse(string value) =>
            Result.Of(() => ColorTranslator.FromHtml(value), $"Incorrect color: {value}.");
    }
}