using System;
using System.IO;
using TagsCloud.ResultPattern;

namespace TagsCloud.FileReader
{
    public class TxtReader : IWordsReader
    {
        public Result<string[]> ReadWords(string path)
        {
            return path.AsResult()
                .Then(File.ReadAllText)
                .Then(x => x.Split(new string[0], StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
