using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult
{
    internal class Drawer: IDrawer<Word>
    {
        private readonly DrawSettings<Word> drawSettings;
        private readonly IEnumerable<ItemToDraw<Word>> itemsToDraws;
        public Drawer(DrawSettings<Word> drawSettings, WordLayouter wordLayouter)
        {
            this.drawSettings = drawSettings;
            itemsToDraws = wordLayouter.GetItemsToDraws();
        }

        public Result<None> DrawItems(string resultFilePath = null)
        {
            var bitmap = new Bitmap(drawSettings.GetImageSize().Width, drawSettings.GetImageSize().Height);
            var g = Graphics.FromImage(bitmap);

            foreach (var item in itemsToDraws)
            {
                var size = item.Height;
                var font = new Font(new FontFamily(drawSettings.GetFontName()), size);

                g.DrawString(
                    item.Body.Value,
                    font,
                    drawSettings.GetBrush(item),
                    item.X, item.Y);
            }

            var fullFileName = drawSettings.GetFullFileName();

            if (fullFileName.IsSuccess)
                bitmap.Save(fullFileName.Value);

            g.Dispose();
            bitmap.Dispose();

            return fullFileName.IsSuccess ? new Result<None>() : new Result<None>(fullFileName.Error);
        }
    }
}