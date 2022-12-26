namespace TagsCloudContainer.Visualisator
{
    public interface IPainter
    {
        public void Paint(List<(Rectangle rectangle, string text)> rectangles);
    }
}
