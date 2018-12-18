using System.Collections.Generic;
using NUnit.Framework;
using ResultOf;
using TagsCloudContainer.Reading;

namespace TagsCloudContainer.Filtering
{
    public class BoringWordsRepository : IBoringWordsRepository
    {
        public IEnumerable<string> Words { get; private set; }


        public Result<None> LoadWords(string inputPath)
        {
            return new TxtWordsReader()
                .ReadWords(inputPath)
                .RefineError("Error reading boring words file")
                .Then(x => { Words = x; });
        }
    }
}