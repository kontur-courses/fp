using ResultOF;
using System.IO;

namespace TagCloud
{
    public class TxtReader : IReader
    {
        public Result<string> ReadAllText(string pathToFile)
        {
            return File.ReadAllText(pathToFile);
        }
    }
}
