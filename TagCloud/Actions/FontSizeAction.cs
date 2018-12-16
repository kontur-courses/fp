using TagCloud.Forms;
using TagCloud.Settings;

namespace TagCloud.Actions
{
    public class FontSizeAction : IUiAction
    {
        private FontSettings fontSettings;
        private SettingsLoader watcher;

        public FontSizeAction(FontSettings fontSettings, SettingsLoader watcher)
        {
            this.fontSettings = fontSettings;
            this.watcher = watcher;
        }

        public string Category => "Settings";
        public string Name => "Font Size";
        public string Description => "Change Font Sizes";

        public void Perform()
        {
            fontSettings = watcher.FontSettings;
            SettingsForm.For(fontSettings).ShowDialog();
        }
    }
}