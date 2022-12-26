using System.Drawing;

namespace TagsCloudVisualization.Saver
{
    internal interface IImageSaver
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }

        public Result<None> Save(Image image);
    }
}
