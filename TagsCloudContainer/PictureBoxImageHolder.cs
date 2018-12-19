using ResultOf;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        public Result<Size> GetImageSize()
        {
            return FailIfNotInitialized()
                .Then(_ => Image.Size);
        }

        public Result<Graphics> StartDrawing()
        {
            return FailIfNotInitialized()
                .Then(_ => Graphics.FromImage(Image))
                .RefineError("Can't start drawing");
        }

        public Result<None> RecreateImage(ImageSettings settings)
        {
            if (settings.Width <= 0 || settings.Height <= 0)
                return Result.Fail<None>("Image sizes must be bigger than 0");
            Image = new Bitmap(settings.Width, settings.Height, PixelFormat.Format24bppRgb);
            return Result.Ok();
        }

        public Result<None> SaveImage(string fileName)
        {
            return FailIfNotInitialized()
                .Then(_ => Image.Save(fileName))
                .RefineError("Can't save image");
        }

        private Result<None> FailIfNotInitialized()
        {
            return Image == null
                ? Result.Fail<None>("Call    PictureBoxImageHolder.RecreateImage before other method call!")
                : Result.Ok();
        }

        public void UpdateUi()
        {
            Refresh();
            Application.DoEvents();
        }
    }
}