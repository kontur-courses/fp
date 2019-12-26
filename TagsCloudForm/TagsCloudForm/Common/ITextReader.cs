using System.Collections.Generic;

namespace TagsCloudForm.Common
{
    public interface ITextReader
    {
        Result<IEnumerable<string>> ReadLines(string fileName);
    }
}
