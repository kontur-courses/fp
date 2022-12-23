using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.InfrastructureUI.Actions
{
    public class FontSettingsAction : IUiAction
    {
        private readonly HashSet<string> fonts;
        private readonly FontSettings settings;

        public FontSettingsAction(FontSettings settings)
        {
            this.settings = settings;
            fonts = new InstalledFontCollection().Families.Select(f => f.Name.ToLower()).ToHashSet();
        }

        public Category Category => Category.Settings;
        public string Name => "Шрифт...";
        public string Description => "";

        public void Perform()
        {
            var minSize = settings.MinEmSize;
            var maxSize = settings.MaxEmSize;
            var font = settings.FontFamily;
            SettingsForm.For(settings).ShowDialog();
            var foundFont = fonts.Contains(settings.FontFamily.ToLower());

            if (settings.MaxEmSize > 0 && settings.MinEmSize > 0
                                       && settings.MaxEmSize >= settings.MinEmSize
                                       && foundFont) return;

            if (settings.MaxEmSize <= 0 || settings.MinEmSize <= 0)
                Error.HandleError<ErrorHandlerUi>("размеры шрифта должны быть положительными");

            if (!foundFont)
                Error.HandleError<ErrorHandlerUi>(
                    $"шрифт {settings.FontFamily} не был найден в системе, поменяйте шрифт");

            if (settings.MaxEmSize < settings.MinEmSize)
                Error.HandleError<ErrorHandlerUi>("MaxSize < MinSize, поменяйте размеры");

            settings.MaxEmSize = settings.MaxEmSize <= 0 || settings.MaxEmSize < settings.MinEmSize
                ? maxSize
                : settings.MaxEmSize;

            settings.MinEmSize = settings.MinEmSize <= 0 || settings.MaxEmSize < settings.MinEmSize
                ? minSize
                : settings.MinEmSize;

            settings.FontFamily = !foundFont ? font : settings.FontFamily;
        }
    }
}