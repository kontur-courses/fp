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
            if (Image != null)
                mainForm.Value.ClientSize = sizeSettings.Size;

            var generationResult = cloudGenerator.GenerateCloud();

            if (!generationResult.IsSuccess)
            {
                MessageBox.Show(generationResult.Error);
                return;
            }

            Image = generationResult.Value;
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
    }
}