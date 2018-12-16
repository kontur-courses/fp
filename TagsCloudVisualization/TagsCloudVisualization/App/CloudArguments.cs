namespace TagsCloudVisualization
{
    public class CloudArguments
    {
        public string FileName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Font { get; set; }

        public CloudArguments(string fileName, int width, int height, string font)
        {
            FileName = fileName;
            Width = width;
            Height = height;
            Font = font;
        }

        public CloudArguments()
        {

        }
    }
}