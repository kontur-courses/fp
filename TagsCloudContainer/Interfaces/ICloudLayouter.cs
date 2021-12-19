using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Interfaces;

public interface ICloudLayouter
{
    Result<Rectangle> PutNextRectangle(Size rectangleSize);

    void Reset();
}
