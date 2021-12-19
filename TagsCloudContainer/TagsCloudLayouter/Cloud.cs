namespace TagsCloudContainer.TagsCloudLayouter
{
    public class Cloud
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }

        public Cloud(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
        }
    }
}
