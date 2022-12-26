using TagCloudContainer.Core.Interfaces;

namespace TagCloudContainer.Configs;

public class TagCloudPlacerConfig : ITagCloudPlacerConfig
{
    public Point FieldCenter { get; set; }
    public SortedList<float, Point> NearestToTheFieldCenterPoints { get; set; }
    public List<Rectangle> PutRectangles { get; set; }
}