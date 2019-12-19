using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface ITextReader
    {
        Result<IEnumerable<string>> GetLines();
    }

}