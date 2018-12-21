using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TagCloud.Interfaces;

namespace TagCloud
{
    public class FileReaderByLines : IFileReader
    {
        public Result<IEnumerable<string>> Read(string path)
        {
            return Result.Of(() => File.ReadAllLines(path, Encoding.Default).AsEnumerable());
        }
    }
}