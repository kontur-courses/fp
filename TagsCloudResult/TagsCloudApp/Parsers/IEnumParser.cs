using System;
using TagsCloudContainer.Results;

namespace TagsCloudApp.Parsers
{
    public interface IEnumParser
    {
        Result<T> Parse<T>(string value) where T : struct, Enum;
    }
}