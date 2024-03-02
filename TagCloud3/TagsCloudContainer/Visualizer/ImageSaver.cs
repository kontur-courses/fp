using ResultOf;
using System.Drawing;

namespace TagsCloudContainer.Visualizer
{
    public class ImageSaver
    {
        public static Result<bool> SaveToFile(Image image, string filePath, string fileFormat)
        {
            if (image == null)
            {
                return Result<bool>.Fail("Can't save. Image is null.");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                return Result<bool>.Fail(@"File path cannot be null or empty: {nameof(filePath)}");
            }

            switch (fileFormat.ToLowerInvariant())
            {
                case "bmp":
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case "png":
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case "jpeg":
                case "jpg":
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case "gif":
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case "tiff":
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Tiff);
                    break;
                default:
                    return Result<bool>.Fail(@"Unsupported file format: {fileFormat}");
            }

            return Result<bool>.Ok(true);
        }
    }
}
