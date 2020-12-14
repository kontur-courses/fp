using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagCloud.Interfaces;

namespace TagCloud
{
    public class WordsReader : IWordsReader
    {
        public Result<List<string>> Get(string path)
        {
            return Result.Of(() =>
            {
                if (!File.Exists(path))
                    return new List<string>();
                using (var fileStream = new StreamReader(path))
                {
                    return fileStream.ReadToEnd().Split('\n').ToList();
                }
            });
        }
    }
}