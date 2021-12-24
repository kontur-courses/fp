using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ResultMonad;
using ResultMonad.Extensions;

namespace TagsCloudImageSaver
{
    public class ImageSaver
    {
        private readonly string directory;
        private readonly string imageName;

        public ImageSaver(string directory, string imageName)
        {
            this.directory = directory;
            this.imageName = imageName;
        }
        
        public void Save(Image image)
        {
            directory.AsResult()
                .Then(dir => GetPath(dir, imageName))
                .SelectMany(_ => DefineFormat(imageName), (path, format) => new { path, format })
                .Then(tuple => image.Save(tuple.path, tuple.format))
                .OnFail(Console.WriteLine);
        }
        
        private Result<ImageFormat> DefineFormat(string imageName)
        {
            var extension = Path.GetExtension(imageName);
            return extension switch
            {
                ".png" => ImageFormat.Png,
                ".jpeg" or ".jpg" => ImageFormat.Jpeg,
                ".bmp" => ImageFormat.Bmp,
                _ => Result.Fail<ImageFormat>($"Can't save image in this {extension} format")
            };
        }

        private static Result<string> GetPath(string directory, string imageName) =>
            directory
                .AsResult()
                .Validate(Directory.Exists, $"No such directory {directory}")
                .Then(dir => Path.Combine(dir, imageName));
    }
}