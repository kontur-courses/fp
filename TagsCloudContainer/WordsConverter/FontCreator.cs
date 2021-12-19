using System.Drawing.Text;
using System.Linq;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.WordsConverter
{
    public class FontCreator : IFontCreator
    {
        private readonly InstalledFontCollection supportedFonts;
        private readonly IAppSettings appSettings;
        private const int MinFontSize = 8;
        private const int MaxFontSize = 28;

        public FontCreator(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
            supportedFonts = new InstalledFontCollection();
        }

        public float GetFontSize(int wordFrequency, int maxWordFrequency)
        {
            var size = MaxFontSize * (wordFrequency / (float)maxWordFrequency);
            if (size < MinFontSize)
                return MinFontSize;

            return size;
        }

        public Result<string> GetFontName()
        {
            return supportedFonts.Families.Any(font => font.Name == appSettings.FontName)
                ? Result.Ok(appSettings.FontName)
                : Result.Fail<string>($"{appSettings.FontName} font is not supported");
        }
    }
}