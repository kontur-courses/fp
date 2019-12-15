using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloud.Interfaces;

namespace TagsCloud
{
	public class LayoutConstructor: ILayoutConstructor
	{
		private readonly ITagsProcessor tagsProcessor;
		private readonly ICloudLayouter layouter;

		public LayoutConstructor(ITagsProcessor tagsProcessor, ICloudLayouter layouter)
		{
			this.tagsProcessor = tagsProcessor;
			this.layouter = layouter;
		}

		public Result<Layout> GetLayout()
		{
			layouter.ResetState();
			var tags = tagsProcessor.GetTags();
			if (!tags.IsSuccess)
				return Result.Fail<Layout>(tags.Error);
			var layoutTags = tags.Value.Select(tag =>
			{
				var tagArea = layouter.PlaceNextRectangle(tag.Area.Size);
				return new Tag(tag.Text, tag.TextSize, tagArea);
			});
			return new Layout(layoutTags);
		}
	}
}