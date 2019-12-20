using System.Drawing;
using System.Windows.Forms;
using TagCloud.CloudLayouter;
using TagCloud.CloudVisualizerSpace.CloudViewConfigurationSpace;
using TagCloud.WordsPreprocessing;

namespace TagCloud.CloudVisualizerSpace
{
    public class CloudVisualizer
    {
        private readonly CloudViewConfiguration cloudViewConfiguration;
        private CloudConfiguration cloudConfiguration;

        public CloudVisualizer(CloudViewConfiguration configuration, CloudConfiguration cloudConfiguration)
        {
            cloudViewConfiguration = configuration;
            this.cloudConfiguration = cloudConfiguration;
        }

        public Result<Bitmap> GetCloud(Word[] words)
        {
            var cloudLayouter = cloudConfiguration.CloudLayouter();
            if (cloudViewConfiguration.ImageSize.Width <= 0 || cloudViewConfiguration.ImageSize.Height <= 0)
                return Result.Fail<Bitmap>("Image size should be a non-negative");

            if (!cloudViewConfiguration.FontFamily.IsSuccess)
                return Result.Fail<Bitmap>(cloudViewConfiguration.FontFamily.Error);

            var image = new Bitmap(cloudViewConfiguration.ImageSize.Width, cloudViewConfiguration.ImageSize.Height);
            var imageRectangle = new Rectangle(Point.Empty, cloudViewConfiguration.ImageSize);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(cloudViewConfiguration.BackgroundColor);
                foreach (var word in words)
                {
                    var font = new Font(cloudViewConfiguration.FontFamily.Value,
                        (float) (word.Frequency * cloudViewConfiguration.ScaleCoefficient + 1));
                    var size = TextRenderer.MeasureText(word.Value, font);
                    var rectangle = cloudLayouter
                        .PutNextRectangle(size)
                        .Then(r =>
                        {
                            graphics.DrawString(word.Value, font, cloudViewConfiguration.GetBrush(),
                                r.Location);
                            return r;
                        });

                    if (!rectangle.IsSuccess || !imageRectangle.Contains(cloudLayouter.WrapperRectangle))
                        return Result.Fail<Bitmap>(
                            "Cloud is not in the image. Please increment image size, or set right cloud settings");
                }
            }

            return Result.Ok(image);
        }
    }
}