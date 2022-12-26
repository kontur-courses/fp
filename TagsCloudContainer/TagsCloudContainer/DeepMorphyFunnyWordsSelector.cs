using CSharpFunctionalExtensions;
using DeepMorphy;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class DeepMorphyFunnyWordsSelector : IFunnyWordsSelector
{
    private static readonly HashSet<string> FunnySpeechPieces = new()
    {
        "числ", "сущ", "прил", "гл", "деепр", "цифра", "рим_цифр", "прич"
    };

    private readonly MorphAnalyzer morphAnalyzer;

    public DeepMorphyFunnyWordsSelector(MorphAnalyzer morphAnalyzer)
    {
        this.morphAnalyzer = morphAnalyzer;
    }

    public Result<IEnumerable<CloudWord>> RecognizeFunnyCloudWords(IEnumerable<string> allWords)
    {
        return Result.Try(() => morphAnalyzer.Parse(allWords))
            .Bind(morphInfos => Result.Success(morphInfos
                .Where(x => FunnySpeechPieces.Contains(x.BestTag["чр"]))
                .Select(x => x.BestTag.HasLemma ? x.BestTag.Lemma : x.Text)
                .GroupBy(x => x)
                .Select(x => new CloudWord(x.Key, x.Count()))
                .ToArray()))
            .Bind(words => Result.SuccessIf(words.Any,
                words.AsEnumerable(), "No one word is match"));
    }
}