using System;
using System.Drawing;
using ResultMonad;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Drawable.Tags.Settings;

namespace TagsCloudVisualization.Drawable.Tags.Factory
{
    public class TagDrawableFactory : ITagDrawableFactory
    {
        private readonly ITagDrawableSettingsProvider _settingsProvider;
        private readonly ILayouter _layouter;

        public TagDrawableFactory(ILayouter layouter, ITagDrawableSettingsProvider settingsProvider)
        {
            _layouter = layouter;
            _settingsProvider = settingsProvider;
        }

        public Result<TagDrawable> Create(Tag tag)
        {
            var height = tag.Weight * _settingsProvider.Font.MaxSize;
            var size = Size.Round(new SizeF(Math.Max(1, height * tag.Word.Length), Math.Max(1, height)));
            return _layouter.PutNextRectangle(size)
                .Then(rectangle => new TagDrawable(tag, rectangle, _settingsProvider));
        }
    }
}