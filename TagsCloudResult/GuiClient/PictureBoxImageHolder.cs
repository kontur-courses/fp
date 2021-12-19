using System.Windows.Forms;
using App;
using App.Infrastructure;
using App.Infrastructure.SettingsHolders;

namespace GuiClient
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        private readonly ICloudGenerator cloudGenerator;
        private readonly IOutputResultSettingsHolder outputResultSettings;
        private readonly IImageSizeSettingsHolder sizeSettings;

        public PictureBoxImageHolder(
            IImageSizeSettingsHolder sizeSettings,
            IOutputResultSettingsHolder outputResultSettings,
            ICloudGenerator cloudGenerator)
        {
            this.sizeSettings = sizeSettings;
            this.outputResultSettings = outputResultSettings;
            this.cloudGenerator = cloudGenerator;
            SizeMode = PictureBoxSizeMode.StretchImage;

        }

        public void GenerateImage()
        {
            if (sizeSettings.Size.IsEmpty)
                sizeSettings.Size = Parent.ClientSize;

            cloudGenerator.GenerateCloud()
                .OnFail(error => MessageBox.Show(error))
                .Then(result => Image = result.Visualization);
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