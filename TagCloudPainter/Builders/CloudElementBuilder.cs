using TagCloudPainter.Common;
using TagCloudPainter.Layouters;
using TagCloudPainter.ResultOf;
using TagCloudPainter.Sizers;

namespace TagCloudPainter.Builders;

public class CloudElementBuilder : ITagCloudElementsBuilder
{
    private readonly ICloudLayouter _cloudLayouter;
    private readonly IWordSizer _wordSizer;

    public CloudElementBuilder(IWordSizer wordSizer, ICloudLayouter cloudLayouter)
    {
        _wordSizer = wordSizer;
        _cloudLayouter = cloudLayouter;
    }

    public Result<IEnumerable<Tag>> GetTags(Dictionary<string, int> dict)
    {
        if (dict is null || dict.Count == 0)
            return Result.Fail<IEnumerable<Tag>>("dict is null or empty");

        var result = new List<Tag>();
        foreach (var (word, count) in dict)
        {
            if (string.IsNullOrWhiteSpace(word) || count < 1)
                return Result.Fail<IEnumerable<Tag>>("word is empty or word count < 1");

            var size = _wordSizer.GetTagSize(word, count);
            if (!size.IsSuccess)
                return Result.Fail<IEnumerable<Tag>>($"{size.Error}");
            var rectangle = _cloudLayouter.PutNextRectangle(size.Value);

            if (!rectangle.IsSuccess)
                return Result.Fail<IEnumerable<Tag>>($"{rectangle.Error}");
            result.Add(new Tag(word, rectangle.Value, count));
        }

        return result;
    }
}