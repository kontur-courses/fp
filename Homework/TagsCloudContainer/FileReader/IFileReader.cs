using System.Collections.Generic;

namespace TagsCloudContainer.FileReader
{
    public interface IFileReader
    {
        public string Extension { get; }
        public Result<IEnumerable<string>> ReadWords(string path);
    }
}