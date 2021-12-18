using System.Drawing;
using System.Linq;
using ResultMonad;

namespace TagsCloudVisualization.Drawable.Tags.Settings
{
    public class FontSettings
    {
        public string Family { get; }
        public int MaxSize { get; }

        private FontSettings(string family, int maxSize)
        {
            Family = family;
            MaxSize = maxSize;
        }

        public static FontSettings Default { get; } = new("Arial", 50);

        public static Result<FontSettings> Create(string fontFamily, int maxSize)
        {
            return Result.Ok()
                .Validate(() => IsValidFont(fontFamily), $"Font {fontFamily} not supported")
                .Validate(() => maxSize > 0, $"Expected positive max size, but actual was {maxSize}")
                .ToValue(new FontSettings(fontFamily, maxSize));
        }

        private static bool IsValidFont(string fontFamily)
        {
            return FontFamily.Families.Any(x => x.Name.Equals(fontFamily));
        }
    }
}