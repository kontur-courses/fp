using System.Collections.Generic;
using FunctionalTools;

namespace TagsCloudGenerator.WordsHandler.Converters
{
    public interface IConverter
    {
        Result<Dictionary<string, int>> Convert(Dictionary<string, int> wordToCount);
    }
}