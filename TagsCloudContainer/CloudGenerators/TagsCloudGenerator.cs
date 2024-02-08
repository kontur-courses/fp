using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.TextMeasures;

namespace TagsCloudContainer.CloudGenerators;

public class TagsCloudGenerator : ITagsCloudGenerator
{
    private readonly ITagTextMeasurer tagTextProvider;
    private readonly ICloudLayouter cloudLayouter;

    public TagsCloudGenerator(ICloudLayouter cloudLayouter, ITagTextMeasurer tagTextProvider)
    {
        this.cloudLayouter = cloudLayouter;
        this.tagTextProvider = tagTextProvider;
    }

    public Result<ITagCloud> Generate(IEnumerable<WordDetails> wordsDetails)
    {
        var clearCloud = cloudLayouter.UpdateCloud();
        if (!clearCloud.IsSuccess)
            return Result.Fail<ITagCloud>(clearCloud.Error);

        var sorted = SortWords(wordsDetails);
        var tags = new List<Tag>();
        foreach (var word in sorted)
        {
            var result = tagTextProvider.Measure(word)
                .Then(cloudLayouter.PutNextRectangle)
                .Then(rectangle => tags.Add(new Tag(rectangle, word)));
            if (!result.IsSuccess)
                return Result.Fail<ITagCloud>($"Ошибка при вычислении положения тега. {result.Error}");
        }

        return new TagsCloud(tags);
    }

    private IEnumerable<string> SortWords(IEnumerable<WordDetails> wordsDetails)
    {
        return wordsDetails
            .OrderByDescending(x => x.Frequency)
            .Select(x => x.Word);
    }
}