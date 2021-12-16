using System.Drawing;
using TagsCloudContainer;

namespace TagsCloudApp.Parsers
{
    public interface IArgbColorParser
    {
        Result<Color> TryParse(string value);
    }
}