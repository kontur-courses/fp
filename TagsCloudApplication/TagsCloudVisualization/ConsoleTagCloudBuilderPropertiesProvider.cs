using System.Drawing;
using System.Linq;
using TextConfiguration;

namespace TagsCloudVisualization
{
    public class ConsoleTagCloudBuilderPropertiesProvider
    {
        private readonly Options options;

        public ConsoleTagCloudBuilderPropertiesProvider(Options options)
        {
            this.options = options;
        }

        public CloudTagProperties GetCloudTagProperties()
        {
            return new CloudTagProperties(new FontFamily(options.FontFamilyName), options.FontSize);
        }

        public VisualizatorProperties GetVisualizatorProperties()
        {
            var imageSize = options.ImageSize.ToList();
            return new VisualizatorProperties(new Size(imageSize[0], imageSize[1]));
        }

        public ConstantTextColorProvider GetConstantTextColorProvider()
        {
            return new ConstantTextColorProvider(Color.FromName(options.FontColorName));
        }

        public ConsoleTagCloudBuilderIOSettings GetIOSettings()
        {
            return new ConsoleTagCloudBuilderIOSettings(options);
        }

        public Point GetCentralPoint()
        {
            var pointCoords = options.CentralPoint.ToList();
            return new Point(pointCoords[0], pointCoords[1]);
        }
    }
}
