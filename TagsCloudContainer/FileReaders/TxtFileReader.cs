using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainer.FileReaders
{
    public class TxtFileReader : IFileReader
    {
        public HashSet<string> SupportedFormats { get; } = new HashSet<string>() { ".txt" };

        public Result<IEnumerable<string>> ReadWordsFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return Result.Fail<IEnumerable<string>>($"File doesn't exist {path}");
            }

            var words = File.ReadLines(path);
            return Result.Ok(words);
        }
    }
}