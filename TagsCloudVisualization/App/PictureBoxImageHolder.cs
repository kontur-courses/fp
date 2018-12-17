using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TagsCloudVisualization.InterfacesForSettings;

namespace TagsCloudVisualization.App
{
    public class PictureBoxImageHolder : PictureBox
    {
        public Bitmap OriginalImage { get; set; }

        public Result<None> FailIfNotInitialized()
        {
            return Image == null || OriginalImage == null ? Result.Fail<None>("Image is missing.") : Result.Ok();
        }

        public void RecreateImage(ITagsCloudSettings tagCloudSettings)
        {
            var imageSize = tagCloudSettings.ImageSettings.ImageSize;
            Image = new Bitmap(imageSize.Width, imageSize.Height, PixelFormat.Format32bppArgb);
        }

        public void RecreateImage(Bitmap image)
        {
            OriginalImage = image;
            Image = new Bitmap(image, Size);
        }

        public void SaveImage(string fileName)
        {
            var result = FailIfNotInitialized();
            if (result.IsSuccess)
                OriginalImage.Save(fileName, ImageFormat.Png);
            else MessageBox.Show(result.Error, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}