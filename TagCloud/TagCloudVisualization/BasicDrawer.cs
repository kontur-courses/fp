using System;
using System.Drawing;

namespace TagCloudVisualization
{
    public class BasicDrawer : IWordDrawer
    {
        public void DrawWord(Graphics graphics, ImageCreatingOptions options, WordInfo wordInfo, Font font)
        {
            if (wordInfo.Rectangle.HasNoValue)
                throw new ArgumentException();

            graphics.DrawString(wordInfo.Word, font, options.Brush, wordInfo.Rectangle.Value);
        }

        public bool Check(WordInfo word) => true;
    }
}
