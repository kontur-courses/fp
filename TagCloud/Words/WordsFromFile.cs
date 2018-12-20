using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloud.Words;

namespace TagsCloud
{
    public class WordsFromFile : IWordCollection
    {
        private readonly string path;

        public WordsFromFile(string path)
        {
            this.path = path;
        }

        public Result<List<string>> GetWords()
        {
            if (File.Exists(path))
                return File.ReadAllLines(path).ToList();
            return Result.Fail<List<string>>("File doesn`t exist");
        }
    }
}