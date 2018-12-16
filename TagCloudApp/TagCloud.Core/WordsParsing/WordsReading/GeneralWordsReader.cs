using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TagCloud.Core.Util;

namespace TagCloud.Core.WordsParsing.WordsReading
{
    public class GeneralWordsReader : IWordsReader
    {
        private readonly IWordsReader[] wordsReaders;

        public GeneralWordsReader(IWordsReader[] wordsReaders)
        {
            this.wordsReaders = wordsReaders;
            AllowedFileExtension = new Regex(@".*$");
        }

        public Regex AllowedFileExtension { get; }

        public Result<IEnumerable<string>> ReadFrom(string path)
        {
            return Result.Of(() => wordsReaders.First(r => r.AllowedFileExtension.Match(path).Success))
                .ReplaceError(err => $"Can't find WordsReader for file \"{path}\". Wrong extension")
                .Then(suitedReader => suitedReader.ReadFrom(path));
        }
    }
}