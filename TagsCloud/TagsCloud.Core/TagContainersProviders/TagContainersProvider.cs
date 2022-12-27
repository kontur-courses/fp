using System.Drawing;
using TagsCloud.Core.Layouters;
using TagsCloud.Core.Settings;
using TagsCloud.Core.TagContainersProviders.TagsPreprocessors;

namespace TagsCloud.Core.TagContainersProviders;

public class TagContainersProvider : ITagContainersProvider
{
	private readonly Graphics graphics = Graphics.FromImage(new Bitmap(1, 1));
	private readonly ISettingsGetter<ImageSettings> imageSettingsProvider;
	private readonly ICloudLayouter layouter;
	private readonly ITagsPreprocessor preprocessor;

	public TagContainersProvider(ICloudLayouter layouter, ITagsPreprocessor preprocessor,
		ISettingsGetter<ImageSettings> imageSettingsProvider)
	{
		this.layouter = layouter;
		this.preprocessor = preprocessor;
		this.imageSettingsProvider = imageSettingsProvider;
	}

	public List<Result<TagContainer>> GetContainers(int? count)
	{
		var imageSettings = imageSettingsProvider.Get();
		var tags = preprocessor.GetTags(count);

		return tags
			.Select(tag => GetNextContainer(tag, imageSettings))
			.TakeWhile(tagResult => tagResult.IsSuccess)
			.ToList();
	}

	private Result<TagContainer> GetNextContainer(Tag tag, ImageSettings imageSettings)
	{
		var fontSize = imageSettings.MinFontSize + (int)Math.Pow(tag.Count, 1.5);

		var size = GetContainerSize(tag, new Font(imageSettings.FontFamily, fontSize));

		return layouter
			.PutNextRectangle(size)
			.Then(rectangle => new TagContainer(tag, rectangle, fontSize));

		return new TagContainer(tag, layouter.PutNextRectangle(size), fontSize);
	}

	private Size GetContainerSize(Tag tag, Font font)
	{
		var sizeF = graphics.MeasureString(tag.Word, font);

		return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
	}
}