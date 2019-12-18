using System.Collections.Generic;

namespace TagsCloudGenerator.WordsHandler.Converters
{
    public interface IConverter
    {
        Result<Dictionary<string, int>> Convert(Dictionary<string, int> wordToCount);
    }
}