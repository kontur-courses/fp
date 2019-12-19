using System.Drawing;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Settings;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization.Drawers
{
    public abstract class WordDrawer
    {
        protected AppSettings appSettings;

        public WordDrawer(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public abstract Result<Bitmap> GetDrawnLayoutedWords(PaintedWord[] layoutedWords);
    }
}