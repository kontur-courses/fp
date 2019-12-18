using System.Collections.Generic;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface ITextSplitter
    {
        Result<IEnumerable<string>> SplitText(string text);
    }
}
