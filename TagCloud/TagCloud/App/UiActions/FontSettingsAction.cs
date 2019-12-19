namespace TagCloud
{
    public class FontSettingsAction : IUiAction
    {
        private readonly FontSettings fontSettings;

        public MenuCategory Category => MenuCategory.Settings;
        public string Name => "Font...";
        public string Description => "Font settings";

        public FontSettingsAction(FontSettings fontSettings)
        {
            this.fontSettings = fontSettings;
        }

        public void Perform()
        {
            SettingsForm.For(fontSettings).ShowDialog();
            fontSettings.ValidateFontSettings().GetValueOrThrow();
        }
    }
}
