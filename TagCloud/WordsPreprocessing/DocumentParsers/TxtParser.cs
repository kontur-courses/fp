using System;
using System.Collections.Generic;
using TagCloud.Interfaces;

namespace TagCloud.WordsPreprocessing.DocumentParsers
{
    /// <summary>
    /// Returns whole text as one string from the StreamReader
    /// </summary>
    public class TxtParser : IDocumentParser
    {
        public HashSet<string> AllowedTypes => new HashSet<string>{".txt"};

        public Result<IEnumerable<string>> GetWords(ApplicationSettings settings)
        {
            var streamResult = settings.GetDocumentStream();
            if (streamResult.IsSuccess)
            {
                var wordsResult = Result.Of(() => (IEnumerable<string>)streamResult.Value.ReadToEnd()
                    .Split(new[] {" ", "\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries));

                streamResult.Value.Dispose();
                return wordsResult;
            }

            return Result.Fail<IEnumerable<string>>("Can not parse words");
        }

        public void Close()
        {
        }
    }
}
