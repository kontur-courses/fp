using System;
using System.Drawing;
using System.Windows.Forms;
using TagsCloudVisualization.AppSettings;
using TagsCloudVisualization.Canvases;

namespace TagsCloudVisualization.FormAction
{
    public class ImageSettingsAction : IFormAction
    {
        public string Category => "Settings";
        public string Name => "Image size";
        public string Description => "Change image size of your tag cloud visualization";

        private ImageSettings imageSettings;
        private readonly ICanvas canvas;
        
        public ImageSettingsAction(ImageSettings imageSettings, ICanvas canvas)
        {
            this.imageSettings = imageSettings;
            this.canvas = canvas;
        }

        public void Perform()
        {
            var prevSettings = imageSettings.Copy();
            SettingsForm.For(imageSettings).ShowDialog();
            try
            {
                var newImageSize = new Size(imageSettings.Width, imageSettings.Height);
                if (newImageSize != canvas.GetImageSize())
                    canvas.RecreateImage(imageSettings);
            }
            catch (Exception)
            {
                imageSettings = prevSettings;
                MessageBox.Show("Image parameters (width and height) must be greater than 0");
            }
        }
    }
}