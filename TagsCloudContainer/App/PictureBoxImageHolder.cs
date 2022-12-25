using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TagsCloudContainer.Infrastructure;
using ResultOf;

namespace TagsCloudContainer
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        public Result<Size> GetImageSize()
        {
            return FailIfNotInitialized()
                .Then(image => image.Size);
        }

        public Result<Graphics> StartDrawing()
        {
            return FailIfNotInitialized()
                .Then(image => Graphics.FromImage(image));
        }

        private Result<Image> FailIfNotInitialized()
        {
            return (Image == null)
                ? Result.Fail<Image>("Call PictureBoxImageHolder.RecreateImage before other method call!")  
                : Result.Ok(Image);
        }

        public void UpdateUi()
        {
            Refresh();
            Application.DoEvents();
        }

        public void RecreateImage(ImageSettings imageSettings)
        {
            Image = new Bitmap(imageSettings.Width, imageSettings.Height, PixelFormat.Format24bppRgb);
        }

        public void SaveImage(string fileName)
        {
            FailIfNotInitialized().Then(image => image.Save(fileName));
        }
    }
}