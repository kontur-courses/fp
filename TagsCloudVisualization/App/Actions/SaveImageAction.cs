using System.Windows.Forms;

namespace TagsCloudVisualization.App.Actions
{
    public class SaveImageAction : IUiAction
    {
        public string Name => "Сохранить картинку как...";
        public string Category => "Файл";
        private readonly PictureBoxImageHolder imageHolder;
        public SaveImageAction(PictureBoxImageHolder imageHolder)
        {
            this.imageHolder = imageHolder;
        }
        public void Perform()
        {
            var result = imageHolder.FailIfNotInitialized()
                .Then(SaveImage)
                .RefineError("Failed to save file.");
            if (!result.IsSuccess)
                MessageBox.Show(result.Error, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Result<None> SaveImage()
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить картинку как...",
                OverwritePrompt = true,
                CheckPathExists = true,
                DefaultExt = "png",
                FileName = "TagCloud.png",
                Filter = "Изображения (*.png)|*.png",
                ShowHelp = true
            };
            return dialog.ShowDialog() != DialogResult.OK ? Result.Ok() : imageHolder.SaveImage(dialog.FileName);
        }
    }
}