using TagCloud.Forms;
using TagCloud.Settings;

namespace TagCloud.Actions
{
    public class ImageSettingAction : IUiAction
    {
        private readonly ImageBox imageBox;
        private ImageSettings imageSettings;
        private SettingsChecker watcher;

        public ImageSettingAction(ImageBox imageBox, ImageSettings imageSettings, SettingsChecker watcher)
        {
            this.imageBox = imageBox;
            this.imageSettings = imageSettings;
            this.watcher = watcher;
        }

        public string Category => "Settings";
        public string Name => "Image";
        public string Description => "Change Image Settings";

        public void Perform()
        {
            imageSettings = watcher.ImageSettings;
            SettingsForm.For(imageSettings).ShowDialog();
            imageBox.RecreateImage(imageSettings);
        }
    }
}