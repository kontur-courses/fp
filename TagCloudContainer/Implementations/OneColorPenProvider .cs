using System.Drawing;
using TagCloudContainer.Api;

namespace TagCloudContainer.Implementations
{
    [CliElement("onecolorpen")]
    public class OneColorPenProvider : IRectanglePenProvider
    {
        private readonly DrawingOptions options;

        public OneColorPenProvider(DrawingOptions options)
        {
            this.options = options;
        }

        public Pen CreatePenForRectangle(Rectangle rectangle)
        {
            return options.Pen;
        }
    }
}