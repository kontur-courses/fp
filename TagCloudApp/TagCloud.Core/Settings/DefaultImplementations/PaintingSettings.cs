using System.Drawing;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Util;

namespace TagCloud.Core.Settings.DefaultImplementations
{
    public class PaintingSettings : IPaintingSettings
    {
        private Color tagColor;
        private Color backgroundColor;

        public PaintingSettings()
        {
            TagColor = Color.Navy;
            BackgroundColorResult = Color.White;
        }

        public Color TagColor
        {
            get => tagColor;
            set
            {
                tagColor = value;
                TagBrushResult = new SolidBrush(value);
            }
        }

        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                BackgroundColorResult = backgroundColor;
            }
        }

        public Result<Color> BackgroundColorResult { get; set; }
        public Result<Brush> TagBrushResult { get; private set; }
    }
}