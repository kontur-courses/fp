using TagsCloudContainer.Infrastucture;

namespace TagsCloudContainer.Algorithm
{
    public interface IRectanglePlacer
    {
        Result<RectangleF> GetPossibleNextRectangle(List<TextRectangle> cloudRectangles, SizeF rectangleSize);
    }
}