using System;
using System.Windows.Forms;
using App.Infrastructure;
using App.Infrastructure.SettingsHolders;

namespace GuiClient
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        private readonly ICloudGenerator cloudGenerator;
        private readonly Lazy<MainForm> mainForm;
        private readonly IOutputResultSettingsHolder outputResultSettings;
        private readonly IImageSizeSettingsHolder sizeSettings;

        public PictureBoxImageHolder(
            IImageSizeSettingsHolder sizeSettings,
            IOutputResultSettingsHolder outputResultSettings,
            ICloudGenerator cloudGenerator,
            Lazy<MainForm> mainForm
        )
        {
            this.sizeSettings = sizeSettings;
            this.outputResultSettings = outputResultSettings;
            this.mainForm = mainForm;
            this.cloudGenerator = cloudGenerator;
        }

        public void GenerateImage()
        {
            UpdateImageSize();

            var generationResult = cloudGenerator.GenerateCloud();

            if (!generationResult.IsSuccess)
            {
                MessageBox.Show(generationResult.Error);
                return;
            }

            if (!generationResult.Value.IsCloudFitToUserSize())
                MessageBox.Show("Cloud did not fit to specified image size");

            Image = generationResult.Value.Visualization;
        }

        public void SaveImage()
        {
            if (Image == null)
            {
                MessageBox.Show("There's no image to save");
                return;
            }

            Image.Save(outputResultSettings.OutputFilePath, outputResultSettings.ImageFormat);
        }

        private void UpdateImageSize()
        {
            if (Image != null)
            {
                if (sizeSettings.Size.Width <= 0 || sizeSettings.Size.Height <= 0)
                    MessageBox.Show("Incorrect image size.");
                else
                    mainForm.Value.ClientSize = sizeSettings.Size;
            }
        }
    }
}