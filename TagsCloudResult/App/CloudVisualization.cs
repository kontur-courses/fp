using System.Drawing;

namespace App
{
    public class CloudVisualization
    {
        public CloudVisualization(
            Bitmap visualization,
            Size cloudRequiredRequiredSize)
        {
            Visualization = visualization;
            CloudRequiredSize = cloudRequiredRequiredSize;
        }

        public Bitmap Visualization { get; }
        public Size CloudRequiredSize { get; }

        public bool IsCloudFitToSpecifiedSize(Size size)
        {
            return size.Width >= CloudRequiredSize.Width
                   && size.Height >= CloudRequiredSize.Height;
        }
    }
}