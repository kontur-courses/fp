using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.ColorGenerators;
using TagsCloud.Visualization.LayoutContainer;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.Drawers
{
    public class Drawer : IDrawer
    {
        private const int OffsetX = 100;
        private const int OffsetY = 100;
        private readonly IColorGenerator colorGenerator;

        public Drawer(IColorGenerator colorGenerator) => this.colorGenerator = colorGenerator;

        public Result<Image> Draw<T>(ILayoutContainer<T> layoutContainer)
        {
            if (!layoutContainer.Items.Any())
                return Result.Fail<Image>("rectangles array can't be empty");

            var (widthWithOffset, heightWithOffset) = (layoutContainer.Size.Width + OffsetX,
                layoutContainer.Size.Height + OffsetY);

            var center = layoutContainer.Center;
            var bitmap = new Bitmap(widthWithOffset, heightWithOffset);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.TranslateTransform(center.X + widthWithOffset / 2, center.Y + heightWithOffset / 2);

            layoutContainer.Draw(graphics, colorGenerator);

            return bitmap;
        }
    }
}