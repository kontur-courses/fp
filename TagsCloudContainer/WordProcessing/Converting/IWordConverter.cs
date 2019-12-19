using ResultOf;
using System.Collections.Generic;

namespace TagsCloudContainer.WordProcessing.Converting
{
    public interface IWordConverter
    {
        Result<IEnumerable<string>> ConvertWords(IEnumerable<string> words);
    }
}