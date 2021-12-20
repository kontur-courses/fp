using System.Linq;
using TagsCloudContainer.Extensions;
using TagsCloudContainer.TagsCloudLayouter;

namespace TagsCloudContainerTests
{
    public static class TagCloudVisualizationTestsHelper
    {
        public static double CalculateCloudRadius(this Cloud cloud)
        {
            return cloud.Rectangles
                .Select(rectangle => rectangle.Location + rectangle.Size / 2)
                .Select(rectangleCenter => rectangleCenter.GetDistance(cloud.Center))
                .Max();
        }
    }
}
