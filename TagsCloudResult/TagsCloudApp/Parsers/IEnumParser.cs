using System;
using TagsCloudContainer;

namespace TagsCloudApp.Parsers
{
    public interface IEnumParser
    {
        Result<T> TryParse<T>(string value) where T : struct, Enum;
    }
}