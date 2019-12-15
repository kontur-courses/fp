using System.Collections.Generic;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface IWordProcessor
    {
        IEnumerable<string> Process(IEnumerable<string> words);
    }
}