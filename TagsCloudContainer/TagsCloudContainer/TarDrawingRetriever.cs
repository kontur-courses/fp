using System.Drawing;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public static class TarDrawingRetriever
{
    public static Result<IEnumerable<TagDrawingItem>> GetTagDrawingItems(
        this IEnumerable<CloudWord> cloudWords,
        ILayouterAlgorithm algorithm,
        Func<CloudWord, Font> fontResolver,
        Func<TagDrawingItem, Size> sizeResolver,
        Func<TagDrawingItem, bool> filter)
    {
        return Result.Success(cloudWords.Select(x => new TagDrawingItem(x.Text, fontResolver(x), Rectangle.Empty))
            .Select(x => x with { Rectangle = new(Point.Empty, sizeResolver(x)) })
            .Select(x => new { TagDrawingItem = x, Result = algorithm.PutNextRectangle(x.Rectangle.Size) })
            .Where(x => x.Result.IsSuccess)
            .Select(x => x.TagDrawingItem with { Rectangle = x.Result.Value })
            .Where(filter)
        );
    }
}