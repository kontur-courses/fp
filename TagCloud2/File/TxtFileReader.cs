using ResultOf;
using System.IO;

using ResultOf;

namespace TagCloud2
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                return Result.Fail<string>("No such file to open");
            }

            return Result.Ok<string>(File.ReadAllText(path));
        }
    }
}
