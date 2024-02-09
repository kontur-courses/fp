namespace TagCloudResult.Layouter
{
    public interface IRectanglesGenerator
    {
        public Result<IEnumerable<RectangleData>> GetRectanglesData();
    }
}
