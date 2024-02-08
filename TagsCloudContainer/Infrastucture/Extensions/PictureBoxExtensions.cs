using System.Drawing.Imaging;
using TagsCloudContainer.Infrastucture.Settings;

namespace TagsCloudContainer.Infrastucture.Extensions
{
    public static class PictureBoxExtensions
    {
        public static Graphics StartDrawing(this PictureBox source)
        {
            return Graphics.FromImage(source.Image);
        }

        public static void UpdateUi(this PictureBox source)
        {
            source.Refresh();
            Application.DoEvents();
        }

        public static void RecreateImage(this PictureBox source, ImageSettings imageSettings)
        {
            source.Image = new Bitmap(imageSettings.Width, imageSettings.Height, PixelFormat.Format24bppRgb);
        }

        public static Result<None> SaveImage(this PictureBox source, string fileName)
        {
            var resultSaving = Result.OfAction(() => source.Image.Save(fileName));

            if (!resultSaving.IsSuccess)
            {
                return resultSaving.RefineError($"Failed to save image on path {fileName}");
            }

            return resultSaving;
        }
    }
}