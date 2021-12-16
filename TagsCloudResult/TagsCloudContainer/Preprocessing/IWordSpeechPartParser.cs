using System.Collections.Generic;

namespace TagsCloudContainer.Preprocessing
{
    public interface IWordSpeechPartParser
    {
        Result<IEnumerable<SpeechPartWord>> ParseWords(IEnumerable<string> words);
    }
}