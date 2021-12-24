using System.Drawing;
using TagsCloudVisualization.DrawableContainers;

namespace TagsCloudVisualization.ImageCreators
{
    internal class ImageCreator : IImageCreator
    {
        private const int OffsetX = 100;
        private const int OffsetY = 100;
        
        public Image Draw(IDrawableContainer drawableContainer)
        {
            var size = drawableContainer.GetMinCanvasSize();
            var (widthWithOffset, heightWithOffset) = (size.Width + OffsetX, size.Height + OffsetY);
            var bitmap = new Bitmap(widthWithOffset, heightWithOffset);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.TranslateTransform(bitmap.Width / 2f, bitmap.Height / 2f);

            foreach (var drawableTag in drawableContainer.GetItems())
                drawableTag.Draw(graphics);

            return bitmap;
        }
    }
}