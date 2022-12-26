using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface IFunnyWordsSelector
{
    Result<IEnumerable<CloudWord>> RecognizeFunnyCloudWords(IEnumerable<string> allWords);
}