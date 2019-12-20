using System.Drawing;
using TagCloud.ErrorHandler;
using TagCloud.Extensions;

namespace TagCloud.Visualization
{
    public class ImageSettings
    {
        private readonly IErrorHandler errorHandler;

        public ImageSettings(IErrorHandler errorHandler)
        {
            this.errorHandler = errorHandler;
        }

        private Size imageSize = new Size(1366, 768);

        public Size ImageSize
        {
            get => imageSize;
            set
            {
                if (value.IsNotCorrectSize())
                    errorHandler.HandleError($"Wrong size parameters: width: {value.Width}, height: {value.Height}");
                else imageSize = value;
            }
        }
    }
}