using System;
using System.Collections.Generic;
using System.Linq;
using ResultMonad;
using ResultMonad.Extensions;
using TagsCloudVisualization.ColorService;
using TagsCloudVisualization.Drawable;
using TagsCloudVisualization.FontService;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.SizeService;

namespace TagsCloudVisualization.DrawableContainers.Builders
{
    internal class DrawableContainerBuilder : IDrawableContainerBuilder
    {
        private readonly ITagFontService fontService;
        private readonly ITagColorService colorService;
        private readonly ITagSizeService sizeService;
        private readonly ILayouter layouter;
        private readonly List<Tag> tagContainer = new();
        private float max = float.MinValue;
        private float min = float.MaxValue;

        public DrawableContainerBuilder(ITagFontService fontService, ITagColorService colorService,
            ITagSizeService sizeService, ILayouter layouter)
        {
            this.fontService = fontService;
            this.colorService = colorService;
            this.sizeService = sizeService;
            this.layouter = layouter;
        }

        public void AddTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
            {
                max = Math.Max(max, tag.Count);
                min = Math.Min(min, tag.Count);
                tagContainer.Add(tag);
            }
        }
        public IDrawableContainer Build()
        {
            var container = new DrawableContainer();
            foreach (var tag in tagContainer.OrderByDescending(tag => tag.Count))
            {
                var drawableTag = CreateDrawableTag(tag)
                    .GetValueOrThrow();
               container.AddDrawable(drawableTag);
            }

            return container;
        }
        
        private Result<DrawableTag> CreateDrawableTag(Tag tag)
        {
            var font = fontService.GetFont(tag, min, max);
            var size = sizeService.GetSize(tag, font);
            var color = colorService.GetColor();
            return layouter.PutNextRectangle(size)
                .Then(rectangle => new DrawableTag(tag, rectangle, font, color));
        }
    }
}