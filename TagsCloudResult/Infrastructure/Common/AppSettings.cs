namespace TagsCloudResult.Infrastructure.Common
{
    public class AppSettings
    {
        public ImageSetting ImageSetting { get; }
        public WordSetting WordsSetting { get; }
        public AlgorithmsSettings AlgorithmsSettings { get; }
        public string Path { get; }

        public AppSettings(ImageSetting imageSetting,
            WordSetting wordSetting,
            AlgorithmsSettings algorithmsSettings,
            string path)
        {
            ImageSetting = imageSetting;
            WordsSetting = wordSetting;
            AlgorithmsSettings = algorithmsSettings;
            Path = path;
        }
    }
}