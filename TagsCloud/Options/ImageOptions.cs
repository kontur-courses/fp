namespace TagsCloud.Options
{
    public class ImageOptions : IImageOptions
    {
        public int Width { get; }
        public int Height { get; }
        public string ImageOutputDirectory { get; }
        public string ImageName { get; }
        public string ImageExtension { get; }

        public ImageOptions(string width, string height, string imageOutputDirectory, string imageName, string imageExtension)
        {
            Width = int.Parse(width);
            Height = int.Parse(height);
            ImageOutputDirectory = imageOutputDirectory;
            ImageName = imageName;
            ImageExtension = imageExtension;
        }
    }
}