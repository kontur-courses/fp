using System.Collections.Generic;

namespace TagsCloudForm.Common
{
    public interface ITextReader
    {
        IEnumerable<string> ReadLines(string fileName);
    }
}
