using System.Drawing;
using TagsCloudContainer.Settings;
using TagsCloudContainer.WordsFilters;

namespace TagsCloudContainer.RectanglesFilters
{
    public class FilterRectanglesWhichAreNotInsideImage : IFilter<Rectangle>
    {
        private readonly IImageSettings imageSettings;

        public FilterRectanglesWhichAreNotInsideImage(IImageSettings settings)
        {
            imageSettings = settings;
        }

        public bool IsCorrect(Rectangle rectangle)
        {
            return new Rectangle(Point.Empty, imageSettings.ImageSize).Contains(rectangle);
        }
    }
}