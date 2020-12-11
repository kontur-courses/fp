using System;
using RTFToTextConverter;
using TagsCloud.ResultPattern;

namespace TagsCloud.FileReader
{
    public class RtfReader : IWordsReader
    {
        public Result<string[]> ReadWords(string path)
        {
            return path.AsResult()
                .Then(x => RTFToText.converting().rtfFromFile(x))
                .Then(x => x.Split(new string[0], StringSplitOptions.RemoveEmptyEntries));
        }
    }
}