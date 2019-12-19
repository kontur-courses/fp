using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ErrorHandling;
using TagCloud.Visualization;

namespace TagCloudForm.Holder
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        public PictureBoxImageHolder(ImageSettings imageSettings)
        {
            RecreateImage(imageSettings);
        }

        public void UpdateUi()
        {
            Refresh();
            Application.DoEvents();
        }

        public Result<None> RecreateImage(ImageSettings imageSettings)
        {
            Image = new Bitmap(imageSettings.ImageSize.Width, imageSettings.ImageSize.Height);
            return Result.Ok<None>(null);
        }

        public void SaveImage(string fileName)
        {
            Image.Save(fileName, ImageFormat.Png);
        }
    }
}