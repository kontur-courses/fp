using System;
using System.Drawing;

namespace TagsCloudGenerator
{
    public class RandomlyCloudPainter : ICloudColorPainter
    {
        protected Color[] Palette;
        protected Random Randomizer;
        protected int GetRandomIndex() => Randomizer.Next(Palette.Length - 1);

        public RandomlyCloudPainter(Color[] colorsPalette, Color backgroundColor)
        {
            if (colorsPalette.Length == 0)
                throw new ArgumentException("Colors palette can't be zero");
            
            Palette = colorsPalette;
            BackgroundColor = backgroundColor;
            Randomizer = new Random();
        }

        public Color GetTagShapeColor()
        {
            return Palette[GetRandomIndex()];
        }

        public Color GetTagTextColor(Color shapeColor)
        {
            Color newColor;

            do
            {
                newColor = Palette[GetRandomIndex()];
            } 
            while (shapeColor == newColor && Palette.Length != 1);

            return newColor;
        }

        public Color BackgroundColor { get; }
    }
}