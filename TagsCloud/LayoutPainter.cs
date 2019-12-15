using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Interfaces;

namespace TagsCloud
{
	public class LayoutPainter: ILayoutPainter
	{
		private readonly ImageSettings imageSettings;
		private readonly IImageHolder imageHolder;
		private readonly Palette palette;
		private readonly FontSettings fontSettings;

		public LayoutPainter(ImageSettings imageSettings, IImageHolder imageHolder, 
							Palette palette, FontSettings fontSettings)
		{
			this.imageSettings = imageSettings;
			this.imageHolder = imageHolder;
			this.palette = palette;
			this.fontSettings = fontSettings;
		}

		public Result<None> PaintTags(Layout layout)
		{
			imageHolder.RecreateImage(imageSettings);
			return Result.Ok()
				.Then(_ => imageHolder.GetGraphics())
				.Then(DrawBackground)
				.Then(_ => layout.Tags
					.Select(t => new Tag(t.Text, t.TextSize,
						ToComputerCoords(t.Area, imageHolder.GetImageSize()))))
				.Then(tags => DrawTags(imageHolder.GetGraphics(), tags));
		}

		private void DrawBackground(Graphics graphics)
		{
			var backgroundColor = new SolidBrush(palette.BackgroundColor);
			graphics.FillRectangle(backgroundColor, 0, 0, imageSettings.Width, imageSettings.Height);
		}

		internal static Rectangle ToComputerCoords(Rectangle rectangle, Size imageSize)
		{
			var xOffset = imageSize.Width / 2;
			var yOffset = -2 * rectangle.Y + imageSize.Height / 2;
			rectangle.Offset(xOffset, yOffset);
			return rectangle;
		}

		private Result<None> DrawTags(Graphics graphics, IEnumerable<Tag> tags)
		{
			foreach (var tag in tags)
			{
				if (!CheckTagFitsIntoImage(tag.Area))
					return Result.Fail<None>(new ArgumentException("Tag cloud didn't fit on image of current size"));
				
				var color = palette.RandomizeColors ? palette.GenerateColor() : palette.TextColor;
				var font = new Font(fontSettings.Font.FontFamily, tag.TextSize);
				graphics.DrawString(tag.Text, font, new SolidBrush(color), tag.Area);
				if (palette.DrawWordRectangle)
					graphics.DrawRectangle(new Pen(color), tag.Area);
				imageHolder.UpdateUi();
			}
			return Result.Ok();
		}

		private bool CheckTagFitsIntoImage(Rectangle tagArea) =>
			tagArea.Left >= 0 && tagArea.Top >= 0 &&
			tagArea.Right <= imageSettings.Width && tagArea.Bottom <= imageSettings.Height;
	}
}