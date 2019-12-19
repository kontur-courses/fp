using System.Collections.Generic;
using TagCloud.Interfaces;

namespace TagCloud.WordsPreprocessing.DocumentParsers
{
    public interface IDocumentParser
    {
        HashSet<string> AllowedTypes { get; }
        Result<IEnumerable<string>> GetWords(ApplicationSettings settings);
        void Close();
    }
}