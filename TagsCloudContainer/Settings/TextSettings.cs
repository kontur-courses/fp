using TagsCloudContainer.WordConverter;
using TagsCloudContainer.WordFilter;

namespace TagsCloudContainer.Settings
{
    public class TextSettings
    {
        public TextSettings(Option option, FilterSettings filterSettings)
        {
            CountWords = option.CountWords;
            WordConverters = Converters.GetConvertersByName(option.Converters).Value;
            WordFilters = new Filters(filterSettings).GetFiltersByName(option.Filters);
        }

        public IFilter[] WordFilters { get; }
        public int CountWords { get; }
        public IWordConverter[] WordConverters { get; }
    }
}