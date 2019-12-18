using System;
using System.Collections.Generic;
using TagCloud.Interfaces;

namespace TagCloud.WordsPreprocessing.DocumentParsers
{
    /// <summary>
    /// Returns whole text as one string from the StreamReader
    /// </summary>
    class TxtParser : IDocumentParser
    {
        public HashSet<string> AllowedTypes => new HashSet<string>{".txt"};

        public IEnumerable<string> GetWords(ApplicationSettings settings)
        {
            var streamResult = settings.GetDocumentStream();
            if (streamResult.IsSuccess)
                return settings.GetDocumentStream().Value.ReadToEnd()
                    .Split(new[] {" ", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            return new string[0];
        }

        public void Close()
        {
        }
    }
}
