using ResultOf;
using System.IO;

using ResultOf;

namespace TagCloud2
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                return Result.Fail<string>("Файл не найден или недоступен!");
            }
        }
    }
}
