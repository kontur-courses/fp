using System.Drawing;
using System.Windows.Forms;
using App;
using App.Implementation.SettingsHolders;

namespace GuiClient.UiActions
{
    public class ImageSettingsAction : IUiAction
    {
        private readonly ImageSizeSettings settings;

        public ImageSettingsAction(ImageSizeSettings settings)
        {
            this.settings = settings;
        }

        public MenuCategory Category => MenuCategory.Settings;
        public string Name => "Изображение...";
        public string Description => "Размеры изображения";

        public Result<None> Perform()
        {
            var oldSize = settings.Size;
            var changeSizeResult = Result.OfAction(() => SettingsForm.For(settings).ShowDialog());

            if (!changeSizeResult.IsSuccess)
                return changeSizeResult.RefineError("Can not change image size");

            var newSize = settings.Size;

            if (IsSizeCorrect(newSize))
                return Result.Ok();

            MessageBox.Show("Image width and height must be greater then 0");
            settings.Size = oldSize;
            return Result.Fail<None>("Incorrect image size");

        }

        private bool IsSizeCorrect(Size size)
        {
            return size.Height > 0 && size.Width > 0;
        }
    }
}