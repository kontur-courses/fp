using System;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer.ResultInfrastructure;
using TagsCloudContainer.Visualization.Interfaces;

namespace TagsCloudContainer.Visualization
{
    public class PngSaver : ISaver
    {
        public Result<None> SaveImage(string path, Bitmap image, Size resolution)
        {
            return GetResizeResult(image, resolution.Width, resolution.Height)
                .Then(resizedImage => GetSaveResult(resizedImage, path, ImageFormat.Png));
        }


        private Result<None> GetSaveResult(Bitmap image, string path, ImageFormat format)
        {
            try
            {
                image.Save(path, format);
                return Result.Ok<None>(null);
            }
            catch (Exception e)
            {
                return Result.Fail<None>(e.Message);
            }
        }

        private Result<Bitmap> GetResizeResult(Bitmap bmp, int width, int height)
        {
            return Result.Of(() => ResizeBitmap(bmp, width, height));
        }

        private Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }
    }
}