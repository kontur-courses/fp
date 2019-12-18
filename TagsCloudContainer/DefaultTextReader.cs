using System.Collections.Generic;
using System.IO;
using ResultOf;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer
{
    public class DefaultTextReader : ITextReader
    {
        public DefaultTextReader()
        {
        }

        public Result<IEnumerable<string>> Read(string path)
        {
            return Result.Of(() => File.ReadAllLines(path) as IEnumerable<string>);
        }
    }
}