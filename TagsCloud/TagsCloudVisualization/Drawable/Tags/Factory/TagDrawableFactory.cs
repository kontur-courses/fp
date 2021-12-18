using System;
using System.Drawing;
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
            _layouter = layouter ?? throw new ArgumentNullException(nameof(layouter));
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        }

        public TagDrawable Create(Tag tag)
        {
            var height = tag.Weight * _settingsProvider.Font.MaxSize;
            var size = Size.Round(new SizeF(Math.Max(1, height * tag.Word.Length), Math.Max(1, height)));
            var rectangle = _layouter.PutNextRectangle(size);
            return new TagDrawable(tag, rectangle, _settingsProvider);
        }
    }
}