using System.Collections.Generic;
using System.IO;

namespace TagsCloudForm.Common
{
    public class TextReader : ITextReader
    {
        public IEnumerable<string> ReadLines(string fileName)
        {
            return File.ReadLines(fileName);
        }
    }
}
