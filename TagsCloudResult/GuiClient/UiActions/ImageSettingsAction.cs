using App;
using App.Implementation.SettingsHolders;

namespace GuiClient.UiActions
{
    public class ImageSettingsAction : IUiAction
    {
        private readonly IImageHolder imageHolder;
        private readonly ImageSizeSettings settings;

        public ImageSettingsAction(IImageHolder imageHolder, ImageSizeSettings settings)
        {
            this.imageHolder = imageHolder;
            this.settings = settings;
        }

        public MenuCategory Category => MenuCategory.Settings;
        public string Name => "Изображение...";
        public string Description => "Размеры изображения";

        public Result<None> Perform()
        {
            return Result.OfAction(() => SettingsForm.For(settings).ShowDialog());
        }
    }
}