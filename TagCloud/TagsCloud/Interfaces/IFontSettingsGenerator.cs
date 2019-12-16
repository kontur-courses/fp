using TagsCloud.FontGenerators;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface IFontSettingsGenerator
    {
        Result<FontSettings> GetFontSizeForCurrentWord((string word, int frequency) wordFrequency, int positionByFrequency, int countWords);
    }
}
