namespace TagsCloudContainer.Settings
{
    public class FileSettings
    {
        public FileSettings(Option option)
        {
            Filename = option.InputFileName;
        }

        public string Filename { get; }
    }
}