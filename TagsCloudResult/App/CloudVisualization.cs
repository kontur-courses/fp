using System.Drawing;

namespace App
{
    public class CloudVisualization
    {
        public CloudVisualization(
            Bitmap visualization,
            Size cloudRequiredRequiredSize,
            Size userRequiredSize)
        {
            Visualization = visualization;
            CloudRequiredSize = cloudRequiredRequiredSize;
            UserRequiredSize = userRequiredSize;
        }

        public Bitmap Visualization { get; }
        public Size CloudRequiredSize { get; }
        public Size UserRequiredSize { get; }

        public bool IsCloudFitToUserSize()
        {
            return UserRequiredSize.Width >= CloudRequiredSize.Width
                   && UserRequiredSize.Height >= CloudRequiredSize.Height;
        }
    }
}