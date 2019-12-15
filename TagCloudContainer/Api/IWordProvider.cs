using System.Collections.Generic;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface IWordProvider
    {
        Result<IEnumerable<string>> GetWords();
    }
}