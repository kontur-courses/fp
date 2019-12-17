using System.Drawing;

namespace TagsCloud.Interfaces
{
	public interface ICloudLayouter
	{
		Result<Rectangle> PlaceNextRectangle(Size rectangleSize);
		void ResetState();
	}
}