using App;
using App.Implementation.SettingsHolders;

namespace GuiClient.UiActions
{
    public class PaletteSettingsAction : IUiAction
    {
        private readonly PaletteSettings palette;

        public PaletteSettingsAction(PaletteSettings palette)
        {
            this.palette = palette;
        }

        public MenuCategory Category => MenuCategory.Settings;
        public string Name => "Палитра...";
        public string Description => "Цвета для облака тегов";

        public Result<None> Perform()
        {
            return Result.OfAction(() => SettingsForm.For(palette).ShowDialog());
        }
    }
}