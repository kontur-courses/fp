using System.Drawing;
using TagsCloudContainer.Common;
using TagsCloudContainer.TextAnalyzing;

namespace TagsCloudContainer
{
    public class Painter
    {
        private readonly ColorSettingsProvider colorSettingsProvider;
        private readonly IImageHolder imageHolder;
        private readonly TagCreator tagCreator;

        public Painter(ColorSettingsProvider colorSettingsProvider, IImageHolder imageHolder, TagCreator tagCreator)
        {
            this.imageHolder = imageHolder;
            this.colorSettingsProvider = colorSettingsProvider;
            this.tagCreator = tagCreator;
        }

        public Result<Graphics> PaintTag(Tag tag, Graphics graphics)
        {
            if (!RectangleIsInsideImage(tag.Rectangle))
                return new Result<Graphics>("Облако тегов не вошло на изображение данного размера." +
                                            " Попробуйте уменьшить шрифт или увеличить размеры изображения");
            graphics.DrawString(tag.Text, tag.Font, new SolidBrush(colorSettingsProvider.ColorSettings.GetNextColor()),
                tag.Rectangle);
            graphics.Save();
            return new Result<Graphics>(null, graphics);
        }

        public Result<Graphics> PaintTags()
        {
            var tags = tagCreator.GetTagsForVisualization();
            using (var graphics = imageHolder.StartDrawing())
            {
                graphics.Clear(colorSettingsProvider.ColorSettings.BackgroundColor);
                var result = new Result<Graphics>(null, graphics);
                foreach (var tag in tags)
                {
                    result = PaintTag(tag, graphics);
                    if (!result.IsSuccess)
                        return result;
                }

                imageHolder.UpdateUi();
                return result;
            }
        }

        private bool RectangleIsInsideImage(Rectangle rectangle)
        {
            var imageSize = imageHolder.GetImageSize();
            return rectangle.Left >= 0 && rectangle.Right <= imageSize.Width && rectangle.Top >= 0 &&
                   rectangle.Bottom <= imageSize.Height;
        }
    }
}