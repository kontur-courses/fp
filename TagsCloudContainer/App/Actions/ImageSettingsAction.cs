using System.Windows.Forms;
using ResultOf;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.App.Actions
{
    public class ImageSettingsAction : IUiAction
    {
        private readonly IImageHolder imageHolder;
        private readonly ImageSettings imageSettings;

        public ImageSettingsAction(IImageHolder imageHolder,
            ImageSettings imageSettings)
        {
            this.imageHolder = imageHolder;
            this.imageSettings = imageSettings;
        }

        public string Category => "Настройки";
        public string Name => "Изображение...";
        public string Description => "Размеры изображения";

        public void Perform()
        {
            SettingsForm.For(imageSettings).ShowDialog();
            var settingResult = imageSettings.CheckIsSettingValid();
            imageSettings
                .CheckIsSettingValid()
                .OnFail((error) =>
                {
                    MessageBox.Show(error);
                    imageSettings.SetDefaultSetting();
                });
            imageHolder.RecreateImage(imageSettings);
        }
    }
}