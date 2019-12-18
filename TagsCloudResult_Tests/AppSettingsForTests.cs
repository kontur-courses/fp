using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult_Tests
{
    public static class AppSettingsForTests
    {
        public static readonly AppSettings Settings = new AppSettings(
            new ImageSetting(240, 240, "Black", "png", "test"),
            new WordSetting("Arial", "Red"),
            new AlgorithmsSettings(true),
            "test.txt"
        );
    }
}