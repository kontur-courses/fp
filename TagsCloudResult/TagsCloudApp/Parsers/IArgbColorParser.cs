using System.Drawing;
using TagsCloudContainer.Results;

namespace TagsCloudApp.Parsers
{
    public interface IArgbColorParser
    {
        Result<Color> Parse(string value);
    }
}