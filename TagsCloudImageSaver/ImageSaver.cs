using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ResultMonad;

namespace TagsCloudImageSaver
{
    public class ImageSaver
    {
        private readonly string directory;
        private readonly string imageName;
        private readonly ImageFormat format;

        public ImageSaver(string directory, string imageName)
        {
            this.directory = directory;
            this.imageName = imageName;
            format = DefineFormat(imageName).GetValueOrThrow();
        }

        private static Result<ImageFormat> DefineFormat(string imageName)
        {
            var extension = Path.GetExtension(imageName);
            return extension switch
            {
                ".png" => ImageFormat.Png,
                ".jpeg" or ".jpg" => ImageFormat.Jpeg,
                ".bmp" => ImageFormat.Bmp,
                _ => Result.Fail<ImageFormat>($"Cant save image in this {extension} format.")
            };
        }

        public void Save(Image image)
        {
            directory
                .AsResult()
                .Validate(Directory.Exists, $"No such directory {directory}")
                .Then(dir => Path.Combine(dir, imageName))
                .Then(path => image.Save(path, format));
        }
    }
}