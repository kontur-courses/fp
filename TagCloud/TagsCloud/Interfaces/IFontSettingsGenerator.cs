using TagsCloud.ErrorHandling;
using TagsCloud.FontGenerators;

namespace TagsCloud.Interfaces
{
    public interface IFontSettingsGenerator
    {
        Result<FontSettings> GetFontSizeForCurrentWord((string word, int frequency) wordFrequency,
            int positionByFrequency, int countWords);
    }
}