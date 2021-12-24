using ResultOf;
using System.IO;

namespace TagCloud2
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadFile(string path)
            => File.Exists(path)
                ? Result.Ok(File.ReadAllText(path))
                : Result.Fail<string>("No such file to open");
    }
}
