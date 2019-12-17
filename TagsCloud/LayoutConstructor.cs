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
			return tagsProcessor.GetTags()
				.Then(tags => tags.Select(tag => layouter.PlaceNextRectangle(tag.Area.Size)
					.Then(tagArea => new Tag(tag.Text, tag.TextSize, tagArea))
					.GetValueOrThrow()))
				.Then(layoutTags => new Layout(layoutTags));
		}
	}
}