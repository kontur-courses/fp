using Result;

namespace TagCloudCreation
{
    public class VerbRemover : PartOfSpeechPreparer
    {
        public override Result<Maybe<string>> PrepareWord(string word, TagCloudCreationOptions options) =>
            ProcessWordByTag(word, (tag, w) => tag == PartOfSpeech.Verb ? Maybe<string>.None : Maybe<string>.From(w));
    }
}
