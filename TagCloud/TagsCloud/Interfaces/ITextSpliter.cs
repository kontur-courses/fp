using System.Collections.Generic;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface ITextSpliter
    {
        Result<IEnumerable<string>> SplitText(string text);
    }
}
