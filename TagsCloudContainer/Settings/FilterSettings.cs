namespace TagsCloudContainer.Settings
{
    public class FilterSettings
    {
        public FilterSettings(Option option)
        {
            LengthForBoringWord = option.SmallestLength;
            FileForBoringWords = option.BoringWordsFileName;
        }

        public int LengthForBoringWord { get; }
        public string FileForBoringWords { get; }
    }
}