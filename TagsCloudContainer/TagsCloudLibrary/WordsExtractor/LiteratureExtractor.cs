using System;
using System.Collections.Generic;
using System.IO;
using CSharpFunctionalExtensions;
using TagsCloudLibrary.MyStem;

namespace TagsCloudLibrary.WordsExtractor
{
    public class LiteratureExtractor : IWordsExtractor
    {
        public Result<IEnumerable<string>> ExtractWords(Stream stream)
        {
            return new MyStemProcess().StreamToWords(stream);
        }
    }
}