using System.IO;
using System.Windows.Forms;
using App;
using App.Implementation.SettingsHolders;

namespace GuiClient.UiActions
{
    public class SaveImageAction : IUiAction
    {
        private readonly IImageHolder imageHolder;
        private readonly OutputResultSettings outputSettings;

        public SaveImageAction(IImageHolder imageHolder, OutputResultSettings outputSettings)
        {
            this.imageHolder = imageHolder;
            this.outputSettings = outputSettings;
        }

        public MenuCategory Category => MenuCategory.File;
        public string Name => "Сохранить...";
        public string Description => "Сохранить изображение в файл";

        public Result<None> Perform()
        {
            return Result.OfAction(() =>
            {
                var
                    dialog = new SaveFileDialog
                    {
                        CheckFileExists = false,
                        InitialDirectory = Path.GetFullPath(outputSettings.OutputFilePath),
                        Filter = "Изображения (*.png;*.jpeg;*.bmp)|*.png;*.jpeg;*.bmp"
                    };

                var res = dialog.ShowDialog();
                if (res == DialogResult.OK)
                {
                    outputSettings.OutputFilePath = dialog.FileName;
                    imageHolder.SaveImage();
                }
            });
        }
    }
}