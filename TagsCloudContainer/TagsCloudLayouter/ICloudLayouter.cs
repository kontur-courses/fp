using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.TagsCloudLayouter;

public interface ICloudLayouter
{
    Result<Rectangle> PutNextRectangle(Size rectangleSize);

    void Reset();
}
