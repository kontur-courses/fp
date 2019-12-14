using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ErrorHandler;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.UI.Actions
{
    public class SaveImageAction : IUiAction
    {
        private readonly IImageHolder imageHolder;
        public string Name { get; }

        public SaveImageAction(IImageHolder imageHolder)
        {
            this.imageHolder = imageHolder;
            Name = "Save";
        }

        public void Perform(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog {Filter = "Images|*.png"};
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                imageHolder
                    .GetImage()
                    .Then(image => image.Save(saveDialog.FileName, ImageFormat.Png));
            }
        }
    }
}