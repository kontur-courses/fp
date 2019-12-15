using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.WordsExtractor
{
    public class LineByLineExtractor : IWordsExtractor
    {
        public Result<IEnumerable<string>> ExtractWords(Stream stream)
        {
            try
            {
                using (var sr = new StreamReader(stream))
                {
                    var words = sr.ReadToEnd().Split('\n').Select(s => s.Trim());
                    return Result.Ok(words);
                }
            }
            catch (Exception e)
            {
                return Result.Failure<IEnumerable<string>>("Cannot extract words from stream. " + e.Message);
            }

        }
    }
}
