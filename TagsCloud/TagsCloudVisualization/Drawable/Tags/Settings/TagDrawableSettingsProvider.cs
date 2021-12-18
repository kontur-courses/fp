using System;
using ResultMonad;
using TagsCloudDrawer.ColorGenerators;
using TagsCloudVisualization.Drawable.Tags.Settings.TagColorGenerator;

namespace TagsCloudVisualization.Drawable.Tags.Settings
{
    public class TagDrawableSettingsProvider : ITagDrawableSettingsProvider
    {
        public FontSettings Font { get; }
        public ITagColorGenerator ColorGenerator { get; }

        public static TagDrawableSettingsProvider Default { get; } = new(FontSettings.Default,
            new RandomTagColorGenerator(new RandomColorGenerator(new Random())));

        private TagDrawableSettingsProvider(FontSettings fontSettings, ITagColorGenerator colorGenerator)
        {
            Font = fontSettings;
            ColorGenerator = colorGenerator;
        }

        public static Result<TagDrawableSettingsProvider> Create(
            FontSettings fontSettings,
            ITagColorGenerator colorGenerator)
        {
            return Result.Ok()
                .ValidateNonNull(fontSettings, nameof(fontSettings))
                .ValidateNonNull(colorGenerator, nameof(colorGenerator))
                .ToValue(new TagDrawableSettingsProvider(fontSettings, colorGenerator));
        }
    }
}