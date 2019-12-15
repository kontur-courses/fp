using System.Collections.Generic;
using System.IO;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.WordsExtractor
{
    public interface IWordsExtractor
    {
        Result<IEnumerable<string>> ExtractWords(Stream stream);
    }
}
