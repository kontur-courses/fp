using System.Collections.Generic;
using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator.Clients.VocabularyParsers
{
    public interface ICloudVocabularyParser
    {
        Result<IEnumerable<string>> GetCloudVocabulary(string cloudVocabularyFilename);
    }
}