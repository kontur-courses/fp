using System.Linq;
using JetBrains.Annotations;
using NHunspell;
using TagCloud.Interfaces;
using TagCloud.Result;

namespace TagCloud
{
    public class InfinitiveCastProcessor : IWordProcessor
    {
        private readonly Hunspell hunspell;

        public InfinitiveCastProcessor(byte[] affixFileData, byte[] dictionaryFileData)
        {
            hunspell = new Hunspell(affixFileData, dictionaryFileData);
        }

        public Result<string> Process(string word)
        {
            var result = hunspell.Stem(word).FirstOrDefault();
            if (result == null)
                return new Result<string>("HUnspell error: no such infinitive words found");
            return new Result<string>(null, result);
        }
    }
}