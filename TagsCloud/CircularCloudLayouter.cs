using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using TagsCloud.Interfaces;

namespace TagsCloud
{
	public class CircularCloudLayouter: ICloudLayouter
	{
		private readonly ISpiral spiral;
		internal readonly List<Rectangle> Rectangles = new List<Rectangle>();

		public CircularCloudLayouter(ISpiral spiral) => this.spiral = spiral;

		public Result<Rectangle> PlaceNextRectangle(Size rectangleSize)
		{
			if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
				return Result.Fail<Rectangle>(new ArgumentException("Size values must be non-negative"));

			var newRectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
			while (Rectangles.Any(rect => newRectangle.Intersects(rect)))
				newRectangle = new Rectangle(spiral.GetNextPoint(), rectangleSize);
			
			Rectangles.Add(newRectangle);
			return newRectangle;
		}

		public void ResetState()
		{
			Rectangles.Clear();
			spiral.ResetState();
		}
	}
}