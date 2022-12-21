using System.Drawing;
using ResultOf;
using TagCloud.Common.Extensions;
using TagCloud.Common.Layouter;
using TagCloud.Common.WeightCounter;

namespace TagCloud.Common.TagsConverter;

public class SimpleTagsConverter : ITagsConverter
{
    private ICloudLayouter layouter;
    private IWeightCounter weightCounter;

    public SimpleTagsConverter(ICloudLayouter layouter, IWeightCounter weightCounter)
    {
        this.layouter = layouter;
        this.weightCounter = weightCounter;
    }

    public Result<IEnumerable<Tag>> ConvertToTags(IEnumerable<string> words, int minFontSize)
    {
        var tagsResult = weightCounter.CountWeights(words).Then(wWord => CreateTags(wWord, minFontSize));
        return !tagsResult.IsSuccess ? tagsResult.RefineError("Can't convert to tags") : tagsResult;
    }

    private Size CalculateSize(string word, Font font)
    {
        var graphics = Graphics.FromImage(new Bitmap(1, 1));
        var sizeF = graphics.MeasureString(word, font);
        return sizeF.ConvertToSize();
    }

    private Result<IEnumerable<Tag>> CreateTags(Dictionary<string, int> counterResultValue, int minFontSize)
    {
        var tags = new List<Tag>();
        var maxWeight = counterResultValue.Values.Max();
        foreach (var (word, weight) in counterResultValue)
        {
            var font = new Font("Arial", maxWeight / (maxWeight - weight + 1) + minFontSize);
            var size = CalculateSize(word, font);
            var layoutBoundsResult = layouter.PutNextRectangle(size);
            if (!layoutBoundsResult.IsSuccess)
            {
                return Result.Fail<IEnumerable<Tag>>(layoutBoundsResult.Error);
            }

            var tag = new Tag(layoutBoundsResult.Value, word, font);
            tags.Add(tag);
        }

        layouter.ClearRectanglesLayout();
        return tags;
    }
}