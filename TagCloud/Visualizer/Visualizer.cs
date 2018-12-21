using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Interfaces;
using TagCloud.IntermediateClasses;
using Point = TagCloud.Layouter.Point;

namespace TagCloud.Visualizer
{
    public class Visualizer : IVisualizer
    {
        private readonly IColorScheme colorScheme;
        private readonly IFontScheme fontScheme;

        public Visualizer(
            IColorScheme colorScheme,
            IFontScheme fontScheme,
            Color backgroundColor,
            Size imageSize)
        {
            this.colorScheme = colorScheme;
            this.fontScheme = fontScheme;
            BackgroundColor = backgroundColor;
            Size = imageSize;
        }

        public Color BackgroundColor { get; set; }
        public Size Size { get; }

        public Result<Image> Visualize(IEnumerable<PositionedElement> objects)
        {
            return PrepareToVisualize(objects)
                .Then(source => source.Select(NormalizePosition))
                .Then(elements => DrawElements(elements, Size))
                .Then(bitmap => (Image) bitmap);
        }

        private Result<IEnumerable<VisualElement>> PrepareToVisualize(IEnumerable<PositionedElement> elements)
        {
            return Result.Of(() => elements.Select(element => colorScheme.Process(element)
                .Then(color => fontScheme.Process(element)
                    .Then(font => (font, color)))
                .Then(fontInfo => new VisualElement(element, fontInfo.Item1, fontInfo.Item2))
                .GetValueOrThrow()));
        }

        private Font AdjustFontSize(Graphics graphics, Font font, Size necessarySize, string word)
        {
            const float sizeIncrementInterval = 0.3f;
            var currentFont = new Font(font.FontFamily, 0.1f);
            var currentSize = graphics.MeasureString(word, currentFont).ToSize();
            while (currentSize.Width < necessarySize.Width
                   && currentSize.Height < necessarySize.Height)
            {
                currentFont = new Font(currentFont.FontFamily, currentFont.SizeInPoints + sizeIncrementInterval);
                currentSize = graphics.MeasureString(word, currentFont).ToSize();
            }

            return new Font(currentFont.FontFamily, currentFont.SizeInPoints - sizeIncrementInterval);
        }

        private VisualElement NormalizePosition(VisualElement element)
        {
            var newPosition = new Point(
                (int) (element.Position.X - element.Size.Width / 2),
                (int) (element.Position.Y - element.Size.Height / 2));
            element.Position = newPosition;
            return element;
        }

        private Bitmap DrawElements(
            IEnumerable<VisualElement> elements,
            Size size)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.TranslateTransform(size.Width / 2, size.Height / 2);
                graphics.Clear(BackgroundColor);
                foreach (var element in elements)
                {
                    if (!IsInBounds(size, element))
                        throw new Exception($"The element is outside the image: {size.Width}x{size.Height}");
                    DrawElement(graphics, element);
                }
            }

            return bitmap;
        }

        private void DrawElement(Graphics graphics, VisualElement element)
        {
            using (var pen = new Pen(element.Color))
            {
                var font = AdjustFontSize(graphics, element.Font, element.Size.ToSize(), element.Word);
                graphics.DrawString(element.Word, font, pen.Brush, ExtractRectangleF(element));
            }
        }

        private bool IsInBounds(Size bounds, VisualElement element)
        {
            var isFitInWidth = Math.Abs(element.Position.X + element.Size.Width / 2) <= bounds.Width / 2;
            var isFitInHeight = Math.Abs(element.Position.Y + element.Size.Height / 2) <= bounds.Height / 2;
            return isFitInWidth && isFitInHeight;
        }

        private RectangleF ExtractRectangleF(VisualElement element)
        {
            var locationF = element.Position.ToPointF();
            var sizeF = element.Size.ToSizeF();
            return new RectangleF(locationF, sizeF);
        }
    }
}