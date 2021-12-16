using System.Collections.Generic;
using TagsCloudContainer.Results;

namespace TagsCloudContainer.Preprocessing
{
    public interface IWordSpeechPartParser
    {
        Result<IEnumerable<SpeechPartWord>> ParseWords(IEnumerable<string> words);
    }
}