namespace TagsCloudContainer.Settings
{
    public class AppSettings : IFilePathProvider, IImageDirectoryProvider
    {
        public AppSettings()
        {
            CreateDefaultSettings();
        }

        public string WordsFilePath { get; set; }
        public string ImagesDirectory { get; set; }

        private void CreateDefaultSettings()
        {
            ImagesDirectory = ".";
            WordsFilePath = "words.txt";
        }
    }
}