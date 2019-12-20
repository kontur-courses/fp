using System.Collections.Generic;
using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Word_Counting
{
    public interface IWordCounter
    {
        Result<Dictionary<string, int>> CountWords(IEnumerable<string> words);
    }
}