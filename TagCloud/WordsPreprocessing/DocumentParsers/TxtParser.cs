using System;
using System.Collections.Generic;
using System.IO;
using TagCloud.Interfaces;

namespace TagCloud.WordsPreprocessing.DocumentParsers
{
    /// <summary>
    /// Returns whole text as one string from the StreamReader
    /// </summary>
    public class TxtParser : IDocumentParser
    {
        public HashSet<string> AllowedTypes => new HashSet<string>{".txt"};
        private StreamReader stream;

        private IEnumerable<string> GetWords()
        {
            while (!stream.EndOfStream)
            {
                foreach (var e in stream.ReadLine().Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries))
                {
                    yield return e;
                }
            }
        }

        public Result<IEnumerable<string>> GetWords(ApplicationSettings settings)
        {
            return settings.GetDocumentStream()
                .Then(r =>
                {
                    stream = r;
                    return GetWords();
                });
        }

        public void Dispose()
        {
            try
            {
                stream.Dispose();
            }
            catch
            {
            }
        }
    }
}
