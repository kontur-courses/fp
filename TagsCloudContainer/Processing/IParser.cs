using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Processing
{
    public interface IParser
    {
        Result<Dictionary<string, int>> ParseWords(string input);
    }
}