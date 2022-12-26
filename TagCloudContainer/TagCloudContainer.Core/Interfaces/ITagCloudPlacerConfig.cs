namespace TagCloudContainer.Core.Interfaces;

public interface ITagCloudPlacerConfig
{
    public Point FieldCenter { get; set; }
    
    public SortedList<float, Point> NearestToTheFieldCenterPoints { get; set; }
    
    public List<Rectangle> PutRectangles { get; set; }
}