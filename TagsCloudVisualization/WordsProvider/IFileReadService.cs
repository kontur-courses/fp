using System.Collections.Generic;
using ResultMonad;

namespace TagsCloudVisualization.WordsProvider
{
    public interface IFileReadService
    {
        Result<IEnumerable<string>> GetFileContent();
    }
}