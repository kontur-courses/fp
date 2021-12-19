using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Visualizers
{
    public class DrawingSettings : IDisposable
    {
        public IEnumerable<Color> PenColors { get; }
        public Color BackgroundColor { get; }
        public Font Font { get; }
        public int Width { get; }
        public int Height { get; }
        public string AlgorithmName { get; }

        public DrawingSettings(IEnumerable<Color> penColor, Color backgroundColor, Font font, int width, int height, string algorithmName)
        {
            PenColors = penColor;
            BackgroundColor = backgroundColor;
            Font = font;
            Width = width;
            Height = height;
            AlgorithmName = algorithmName;
        }

        public void Dispose()
        {
            Font?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
