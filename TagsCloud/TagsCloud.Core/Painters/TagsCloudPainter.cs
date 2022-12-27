using System.Drawing;
using TagsCloud.Core.Settings;
using TagsCloud.Core.TagContainersProviders;

namespace TagsCloud.Core.Painters;

public class TagsCloudPainter : ITagsCloudPainter
{
	private readonly ISettingsGetter<ImageSettings> settingsProvider;

	public TagsCloudPainter(ISettingsGetter<ImageSettings> settingsProvider)
	{
		this.settingsProvider = settingsProvider;
	}

	public Result<Bitmap> Draw(List<TagContainer> tagContainers)
	{
		var imageSettings = settingsProvider.Get();
		var cloudBorder = GetCloudBorder(tagContainers, 100);

		if (cloudBorder.Width > 20_000 || cloudBorder.Height > 20_000)
			return Result.Fail<Bitmap>("Tag cloud occupied to much spaces, reduce tags count or font size");

		if (!CloudPlacedInImage(cloudBorder.Size, imageSettings))
			return Result
				.Fail<Bitmap>(
					$"Tag cloud did not placed on the image of the given size, minimal image size should be: ({cloudBorder.Width}, {cloudBorder.Height})");

		var image = imageSettings.AutoSize
			? new Bitmap(cloudBorder.Width, cloudBorder.Height)
			: new Bitmap(imageSettings.ImageSize.Width, imageSettings.ImageSize.Height);

		using var backgroundBrush = new SolidBrush(imageSettings.Pallet.BackgroundColor);
		using var fontBrush = new SolidBrush(imageSettings.Pallet.GetNextColor());
		using var graphics = Graphics.FromImage(image);

		graphics.FillRectangle(backgroundBrush, new Rectangle(new Point(0, 0), image.Size));

		foreach (var tagContainer in tagContainers)
		{
			var place = new Rectangle(new Point(
					tagContainer.Border.X + image.Width / 2,
					tagContainer.Border.Y + image.Height / 2),
				tagContainer.Border.Size);
			using var font = new Font(imageSettings.FontFamily, tagContainer.FontSize);

			graphics.DrawString(
				tagContainer.Tag.Word,
				font,
				fontBrush,
				place);

			fontBrush.Color = imageSettings.Pallet.GetNextColor();
		}

		return image;
	}

	private static Rectangle GetCloudBorder(IReadOnlyCollection<TagContainer> tagContainers, int padding = 0)
	{
		var minX = tagContainers.Min(t => t.Border.Left);
		var minY = tagContainers.Min(t => t.Border.Top);

		var maxX = tagContainers.Max(t => t.Border.Right + padding * 2);
		var maxY = tagContainers.Max(t => t.Border.Bottom + padding * 2);

		var width = maxX - minX;
		var height = maxY - minY;

		return new Rectangle(0, 0, width, height);
	}

	private static bool CloudPlacedInImage(Size cloudSize, ImageSettings settings)
	{
		if (settings.AutoSize) return true;

		var imageSize = settings.ImageSize;

		return cloudSize.Width <= imageSize.Width ||
		       cloudSize.Height <= imageSize.Height;
	}
}