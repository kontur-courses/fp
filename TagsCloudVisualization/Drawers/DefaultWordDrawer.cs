using System;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Settings;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization.Drawers
{
    public class DefaultWordDrawer : WordDrawer
    {
        public DefaultWordDrawer(AppSettings appSettings) : base(appSettings)
        {
        }

        public override Result<Bitmap> GetDrawnLayoutedWords(PaintedWord[] paintedWords)
        {
            var bitmap = new Bitmap(appSettings.ImageSettings.Width, appSettings.ImageSettings.Height);
            var graphicsResult = ResultExt.Of(() => Graphics.FromImage(bitmap));
            if (!graphicsResult.IsSuccess)
                return ResultExt.Fail<Bitmap>(graphicsResult.Error);
            var backgroundBrush = new SolidBrush(appSettings.Palette.BackgroundColor);
            var fontBrush = new SolidBrush(appSettings.Palette.FontColor);
            var stringFormat = new StringFormat {LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center};
            graphicsResult.Value.FillRectangle(backgroundBrush, 0, 0, appSettings.ImageSettings.Width,
                appSettings.ImageSettings.Height);
            foreach (var paintedWord in paintedWords)
            {
                if (paintedWord.Position.GetCornerPoints().Any(IsPointOutOfScreen))
                    return ResultExt.Fail<Bitmap>("Current tag cloud is too big for this screen size");
                var font = GetScaledFontFor(paintedWord);
                graphicsResult.Value
                    .DrawString(paintedWord.Value, font, fontBrush, paintedWord.Position, stringFormat);
            }
            return bitmap.AsResult();
        }

        private bool IsPointOutOfScreen(Point point)
        {
            return point.X < 0
                   || point.Y < 0
                   || point.X > appSettings.ImageSettings.Width
                   || point.Y > appSettings.ImageSettings.Height;
        }

        private Font GetScaledFontFor(PaintedWord layoutedWord)
        {
            return new Font(appSettings.Font.FontFamily, layoutedWord.Position.Size.Height, appSettings.Font.Style);
        }
    }
}