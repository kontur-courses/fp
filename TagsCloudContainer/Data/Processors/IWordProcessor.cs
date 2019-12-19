using System.Collections.Generic;
using TagsCloudContainer.Functional;

namespace TagsCloudContainer.Data.Processors
{
    public interface IWordProcessor
    {
        Result<IEnumerable<string>> Process(IEnumerable<string> words);
    }
}