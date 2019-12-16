using System.Collections.Generic;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface IWordStream
    {
        Result<IEnumerable<string>> GetWords(string path);
    }
}
