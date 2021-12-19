using System.Drawing;

namespace TagsCloudContainer.Layouter
{
    public interface ICloudLayouter
    {
        public Result<Rectangle> PutNextRectangle(Size size);
    }
}