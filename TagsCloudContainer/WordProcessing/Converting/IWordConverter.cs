using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.WordProcessing.Converting
{
    public interface IWordConverter
    {
        Result<IEnumerable<string>> ConvertWords(IEnumerable<string> words);
    }
}