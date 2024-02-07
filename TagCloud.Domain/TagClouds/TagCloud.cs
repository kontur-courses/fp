using System.Drawing;

public class TagCloud : ITagCloud
{
    private readonly IWordExtractor wordExtractor;
    private readonly IFrequencyDictionaryBuilder<string> frequencyBuilder;
    private readonly ITagCloudLayouter layouter;
    private readonly ITagCloudRenderer renderer;
    private readonly IOptionsValidator validator;
    private readonly DomainOptions domainOptions;

    public TagCloud(
        ITagCloudLayouter layouter,
        ITagCloudRenderer renderer,
        IWordExtractor wordExtractor,
        IFrequencyDictionaryBuilder<string> frequencyBuilder,
        DomainOptions domainOptions,
        IOptionsValidator validator)
    {
        this.layouter = layouter;
        this.renderer = renderer;
        this.wordExtractor = wordExtractor;
        this.frequencyBuilder = frequencyBuilder;
        this.domainOptions = domainOptions;
        this.validator = validator;
    }

    public Result<Bitmap> CreateCloud(string text)
    {
        return validator.ValidateOptions(domainOptions)
            .Then(none => wordExtractor.Extract(text))
            .Then(words => frequencyBuilder.Build(words))
            .Then(freqDict => CreateCloud(freqDict));
    }

    private Result<Bitmap> CreateCloud(Dictionary<string, int> frequencyDict)
    {
        var frequencyPairs = frequencyDict.OrderByDescending(x => x.Value)
                                .ToArray();

        var tagCloudOptions = domainOptions.TagCloudOptions;
        if (tagCloudOptions.MaxTagsCount != -1)
            frequencyPairs = frequencyPairs.Take(tagCloudOptions.MaxTagsCount).ToArray();

        var minFreq = frequencyPairs[^1].Value;
        var maxFreq = frequencyPairs[0].Value;

        var layoutResult = Result.Of(() => new List<WordLayout>());

        foreach (var pair in frequencyPairs)
        {
            layoutResult.Then(layouts => {
                var result = LayoutWord(pair.Key, pair.Value, minFreq, maxFreq);
                if (result.IsSuccess)
                    layouts.Add(result.Value);
                return result.IsSuccess ? layouts : Result.Fail<List<WordLayout>>(result.Error);
                });
        }

        return  layoutResult.Then(layouts => renderer.Render(layouts.ToArray()));
    }

    private Result<WordLayout> LayoutWord(string word, int frequency, int minFrequency, int maxFrequency)
    {
        // множитель на который умножается размер шрифта, в зависимости от частоты встречаемости слова. 
        var sizeMultiplier = TagCloudHelpers.GetMultiplier(frequency, minFrequency, maxFrequency);
        var renderOptions = domainOptions.RenderOptions;
        var fontSize = GetFontSize(sizeMultiplier, renderOptions.MinFontSize, renderOptions.MaxFontSize);

        return Result.Of(() => renderer.GetStringSize(word, fontSize))
            .Then(size => layouter.PutNextRectangle(size))
            .Then(rect => new WordLayout(word, rect, fontSize));
    }

    private int GetFontSize(double freqMultiplier, int minFontSize, int maxFontSize)
    {
        var size = minFontSize + (maxFontSize - minFontSize) * freqMultiplier;
        return (int)size;
    }
}