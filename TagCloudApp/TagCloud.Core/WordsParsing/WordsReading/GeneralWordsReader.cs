using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TagCloud.Core.Util;

namespace TagCloud.Core.WordsParsing.WordsReading
{
    public class GeneralWordsReader
    {
        private readonly IWordsReader[] wordsReaders;

        public GeneralWordsReader(IWordsReader[] wordsReaders)
        {
            this.wordsReaders = wordsReaders;
        }

        public Result<IEnumerable<string>> ReadFrom(string path)
        {
            var suitedReaderResult = Result
                .Of(() => wordsReaders.First(r => r.AllowedFileExtension.Match(path).Success))
                .ReplaceError(err => $"Can't find WordsReader for file \"{path}\". Wrong extension");
            if (!suitedReaderResult.IsSuccess)
                return new Result<IEnumerable<string>>(suitedReaderResult.Error);

            return Result.Of(() => new FileStream(path, FileMode.Open))
                .Then(stream => suitedReaderResult.Value.ReadFrom(stream));
        }
    }
}