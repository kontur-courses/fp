using System.Drawing;

namespace TagsCloudVisualization.Saver
{
    internal class ImageSaver : IImageSaver
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FileExtension{ get; set; }

        public Result<None> Save(Image image)
        {
            if (image == null)
                return Result.Fail<None>("Image to save cannot be null");

            image.Save($"{Path}{FileName}{FileExtension}");

            return Result.Ok();
        }

        public ImageSaver(string path, string fileName, string fileExtension)
        {
            Path = path;
            FileName = fileName;
            FileExtension = fileExtension;
        }
    }
}
