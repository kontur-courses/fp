using DeepMorphy;
using FluentResults;
using TagCloud.Abstractions;

namespace TagCloud;

public class MorphWordsProcessor : IWordsProcessor
{
    private readonly MorphAnalyzer morph;
    private readonly IEnumerable<string> partsOfSpeech;

    public MorphWordsProcessor(IEnumerable<string> partsOfSpeech)
    {
        this.partsOfSpeech = partsOfSpeech;
        morph = new MorphAnalyzer(true);
    }

    public Result<IEnumerable<string>> Process(IEnumerable<string> words)
    {
        var infos = morph.Parse(words).ToArray();
        words = infos
            .Where(i => i.Tags.Any())
            .Where(i => partsOfSpeech.Any(b => i.BestTag.Has(b)))
            .Select(i => i.BestTag.Lemma);
        return Result.Ok(words);
    }
}