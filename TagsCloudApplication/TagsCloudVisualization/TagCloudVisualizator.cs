using System.Drawing;
using TagsCloudLayout.CloudLayouters;
using System.Collections.Generic;
using TextConfiguration;

namespace TagsCloudVisualization
{
    public class TagCloudVisualizator : ITagCloudVisualizator
    {
        private readonly VisualizatorProperties properties;
        private readonly ITextColorProvider colorGenerator;
        private readonly ICloudLayouter layouter;

        public TagCloudVisualizator(
            VisualizatorProperties properties,
            ITextColorProvider colorGenerator,
            ICloudLayouter layouter)
        {
            this.properties = properties;
            this.colorGenerator = colorGenerator;
            this.layouter = layouter;
        }

        public Result<Bitmap> VisualizeCloudTags(IReadOnlyCollection<CloudTag> cloudTags)
        {
            var bitmap = new Bitmap(properties.ImageSize.Width, properties.ImageSize.Height);
            var graphics = Graphics.FromImage(bitmap);

            return Result.Ok()
                .Then((_) =>
                {
                    foreach (var cloudTag in cloudTags)
                    {
                        var boundingBoxSize = graphics.MeasureString(cloudTag.Word, cloudTag.Font).ToSize();

                        var rectangle = layouter.PutNextRectangle(boundingBoxSize).GetValueOrThrow();
                        graphics.DrawString(cloudTag.Word,
                            cloudTag.Font,
                            new SolidBrush(colorGenerator.GetTextColor(cloudTag.Word, rectangle)),
                            rectangle.Location);
                    }
                })
            .Then((_) => bitmap);
        }
    }
}
