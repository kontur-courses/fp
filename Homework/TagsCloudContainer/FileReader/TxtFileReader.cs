using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainer.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public string Extension => ".txt";

        public Result<IEnumerable<string>> ReadWords(string path)
        {
            if (!File.Exists(path))
                return Result.Fail<IEnumerable<string>>($"Файла '{path} не существует'");
            return Result.Ok(File.ReadLines(path));
        }
    }
}