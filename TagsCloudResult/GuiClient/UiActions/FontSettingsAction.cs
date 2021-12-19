using System.Windows.Forms;
using App;
using App.Implementation.SettingsHolders;

namespace GuiClient.UiActions
{
    internal class FontSettingsAction : IUiAction
    {
        private readonly IImageHolder imageHolder;
        private readonly FontSettings settings;

        public FontSettingsAction(IImageHolder imageHolder, FontSettings settings)
        {
            this.imageHolder = imageHolder;
            this.settings = settings;
        }

        public MenuCategory Category => MenuCategory.Settings;
        public string Name => "Шрифт...";
        public string Description => "Шрифты для тегов";

        public Result<None> Perform()
        {
            return Result.OfAction(() => SettingsForm.For(settings).ShowDialog());
        }
    }
}