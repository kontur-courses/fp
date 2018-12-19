using System.Collections.Generic;
using System.IO;

namespace TagsCloud
{
    public class WordsFromFile : IWordCollection
    {
        private readonly string path;

        public WordsFromFile(string path)
        {
            this.path = path;
        }

        public Result<IEnumerable<string>> GetWords()
        {
            if (File.Exists(path))
                return File.ReadAllLines(path);
            return Result.Fail<IEnumerable<string>>("File doesn`t exist");
        }
    }
}