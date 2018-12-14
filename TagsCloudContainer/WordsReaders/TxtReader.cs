using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TagsCloudContainer.WordsReaders
{
    public class TxtReader : IWordsReader
    {
        public Result<IEnumerable<string>> GetWords(string filename)
        {
            return Result.Of(() => File.ReadLines(filename, Encoding.UTF8));
        }
    }
}