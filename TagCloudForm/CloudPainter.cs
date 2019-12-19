using ErrorHandling;
using TagCloud.ErrorHandler;
using TagCloud.Visualization;
using TagCloudForm.Holder;

namespace TagCloudForm
{
    public class CloudPainter
    {
        private readonly IImageHolder imageHolder;
        private readonly ImageSettings imageSettings;
        private readonly CloudVisualization cloudVisualization;
        private readonly IErrorHandler errorHandler;

        public CloudPainter(IImageHolder imageHolder,
            ImageSettings imageSettings, CloudVisualization cloudVisualization, IErrorHandler errorHandler)
        {
            this.imageHolder = imageHolder;
            this.imageSettings = imageSettings;
            this.cloudVisualization = cloudVisualization;
            this.errorHandler = errorHandler;
        }

        public void Paint()
        {
            imageHolder.RecreateImage(imageSettings)
                .Then(_ => imageHolder.Image = cloudVisualization.Visualize().GetValueOrThrow())
                .Then(_ => imageHolder.UpdateUi())
                .OnFail(error => errorHandler.HandleError(error));
        }
    }
}