using ResultOf;
using System.IO;

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

            return Result.Ok(File.ReadAllText(path));
        }
    }
}
