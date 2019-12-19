namespace TagCloud
{
    public class ImageSettingsAction : IUiAction
    {
        private readonly IImageHolder imageHolder;
        private readonly ImageSettings imageSettings;

        public MenuCategory Category => MenuCategory.Settings;
        public string Name => "Image...";
        public string Description => "Image size, cloud center";

        public ImageSettingsAction(IImageHolder imageHolder,
            ImageSettings imageSettings)
        {
            this.imageHolder = imageHolder;
            this.imageSettings = imageSettings;
        }

        public void Perform()
        {
            SettingsForm.For(imageSettings).ShowDialog();
            imageSettings.ValidateImageSettings().GetValueOrThrow();
            imageHolder.RecreateImage(imageSettings);
        }
    }
}
