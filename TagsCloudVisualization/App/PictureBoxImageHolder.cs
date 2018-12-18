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
            return new Result<None>().CheckCondition(() => Image == null || OriginalImage == null, "Image is missing.");
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

        public Result<None> SaveImage(string fileName)
        {
            return FailIfNotInitialized()
                .Then(() => OriginalImage.Save(fileName, ImageFormat.Png), "Invalid path to file specified.");
        }
    }
}