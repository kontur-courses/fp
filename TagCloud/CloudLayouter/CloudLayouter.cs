using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Extensions;
using TagCloud.PointGenerator;

namespace TagCloud.CloudLayouter;
public class CloudLayouter : ICloudLayouter
{
    private readonly List<RectangleF> tagCloud;
    private readonly IPointGenerator pointGenerator;
    public RectangleF CloudRectangle { get; private set; }

    public CloudLayouter(IPointGenerator pointGenerator)
    {
        tagCloud = new List<RectangleF>();
        this.pointGenerator = pointGenerator;
    }

    public Result<RectangleF> PutNextRectangle(SizeF rectangleSize)
    {
        return rectangleSize.AsResult()
            .Validate(s => s.Height > 0 && s.Width > 0, "Height and weight should be positive")
            .Then(GetNextRectangle)
            .Then(r =>
            {
                tagCloud.Add(r);
                UpdateCloudBorders(r);
                return r;
            });
    }
    
    private void UpdateCloudBorders(RectangleF newTag)
    {
        CloudRectangle = RectangleF.Union(CloudRectangle, newTag);
    }

    private RectangleF GetNextRectangle(SizeF size)
    {
        var points = pointGenerator.GetPoints(size);
        var rectangles = points.Select(p => p.GetRectangle(size));
        var suitableRectangle = rectangles.First(r => !IsIntersectWithCloud(r));
        return suitableRectangle;
    }

    private bool IsIntersectWithCloud(RectangleF newTag) => tagCloud.Any(tag => tag.IntersectsWith(newTag));
}