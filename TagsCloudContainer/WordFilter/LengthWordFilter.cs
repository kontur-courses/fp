using TagsCloudContainer.Settings;

namespace TagsCloudContainer.WordFilter
{
    public class LengthWordFilter : IFilter
    {
        private readonly FilterSettings filterSettings;

        public LengthWordFilter(FilterSettings filterSettings)
        {
            this.filterSettings = filterSettings;
        }

        public bool Validate(string word)
        {
            return word.Length > filterSettings.LengthForBoringWord;
        }
    }
}