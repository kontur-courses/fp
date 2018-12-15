using Result;

namespace TagCloudCreation
{
    public class PrepositionRemover : PartOfSpeechPreparer
    {
        public override Result<Maybe<string>> PrepareWord(string word, TagCloudCreationOptions options) =>
            ProcessWordByTag(word,
                             (tag, w) => tag == PartOfSpeech.Preposition ? Maybe<string>.None : Maybe<string>.From(w));
    }
}
