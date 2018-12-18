using System.Drawing;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Util;

namespace TagCloud.Core.Settings.DefaultImplementations
{
    public class PaintingSettings : IPaintingSettings
    {
        private Color tagColor;

        public PaintingSettings()
        {
            TagColor = Color.Navy;
            BackgroundColor = Color.White;
        }

        public Color TagColor
        {
            get => tagColor;
            set
            {
                tagColor = value;
                TagBrush = new SolidBrush(value);
            }
        }

        public Color BackgroundColor { get; set; }
        public Brush TagBrush { get; private set; }
    }
}