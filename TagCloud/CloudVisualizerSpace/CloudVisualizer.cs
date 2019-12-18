using System.Drawing;
using System.Windows.Forms;
using TagCloud.CloudVisualizerSpace.CloudViewConfigurationSpace;
using TagCloud.WordsPreprocessing;

namespace TagCloud.CloudVisualizerSpace
{
    public class CloudVisualizer
    {
        private CloudViewConfiguration cloudViewConfiguration; 

        public CloudVisualizer(CloudViewConfiguration configuration)
        {
            cloudViewConfiguration = configuration;
        }

        public Result<Bitmap> GetCloud(Result<Word[]> words)
        {
            var cloudLayouter = cloudViewConfiguration.CloudLayouter();
            if (cloudViewConfiguration.ImageSize.Width <= 0 || cloudViewConfiguration.ImageSize.Height <= 0)
                return Result.Fail<Bitmap>("Image size should be a non-negative");

            if (!words.IsSuccess)
            {
                return Result.Fail<Bitmap>(words.Error);
            }

            if (!cloudViewConfiguration.FontFamily.IsSuccess)
                return Result.Fail<Bitmap>(cloudViewConfiguration.FontFamily.Error);

            var image = new Bitmap(cloudViewConfiguration.ImageSize.Width, cloudViewConfiguration.ImageSize.Height);
            var imageRectangle = new Rectangle(Point.Empty, cloudViewConfiguration.ImageSize);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(cloudViewConfiguration.BackgroundColor);
                foreach (var word in words.Value)
                {
                    var font = new Font(cloudViewConfiguration.FontFamily.Value,
                        (float)(word.Frequency * cloudViewConfiguration.ScaleCoefficient + 1));
                    var size = TextRenderer.MeasureText(word.Value, font);
                    var rectangle = cloudLayouter.PutNextRectangle(size);
                    if (rectangle.IsSuccess)
                        graphics.DrawString(word.Value, font, cloudViewConfiguration.GetBrush(), rectangle.Value.Location);

                    if (!imageRectangle.Contains(cloudLayouter.WrapperRectangle))
                        return Result.Fail<Bitmap>("Cloud is not in the image. Please increment image size, or set right cloud settings");
                }
            }

            return Result.Ok(image);
        }
    }
}
