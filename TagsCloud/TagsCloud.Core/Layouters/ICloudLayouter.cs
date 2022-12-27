using System.Drawing;

namespace TagsCloud.Core.Layouters;

public interface ICloudLayouter
{
	public Result<Rectangle> PutNextRectangle(Size rectangleSize);
}