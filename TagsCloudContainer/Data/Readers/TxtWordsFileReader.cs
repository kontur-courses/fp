using System.Collections.Generic;
using System.IO;
using TagsCloudContainer.Functional;

namespace TagsCloudContainer.Data.Readers
{
    public class TxtWordsFileReader : IFileFormatReader
    {
        public IEnumerable<string> Extensions { get; } = new[] {".txt"};
        public Result<IEnumerable<string>> ReadAllWords(string path)
        {
            return Result.Of(() => File.ReadAllLines(path) as IEnumerable<string>);
        }
    }
}