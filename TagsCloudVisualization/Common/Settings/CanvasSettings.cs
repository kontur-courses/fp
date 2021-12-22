using System;
using System.Drawing;

namespace TagsCloudVisualization.Common.Settings
{
    public class CanvasSettings : ICanvasSettings
    {
        private int width;
        private int height;

        public int Width
        {
            get => width;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("ширина генерируемого изображения должна быть больше 0.");
                width = value;
            }
        }

        public int Height
        {
            get => height;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("высота генерируемого изображения должна быть больше 0.");
                height = value;
            }
        }

        public Color BackgroundColor { get; set; }


        public CanvasSettings()
        {
            Width = 1920;
            Height = 1080;
            BackgroundColor = Color.DimGray;
        }

        public CanvasSettings(int width, int height, Color backgroundColor)
        {
            Width = width;
            Height = height;
            BackgroundColor = backgroundColor;
        }
    }
}