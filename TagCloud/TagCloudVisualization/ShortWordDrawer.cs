using System;
using System.Drawing;

namespace TagCloudVisualization
{
    /// <summary>
    ///     Purely for example
    /// </summary>
    public class ShortWordDrawer : IWordDrawer
    {
        public void DrawWord(Graphics graphics, ImageCreatingOptions options, WordInfo wordInfo, Font font)
        {
            if (wordInfo.Rectangle.HasNoValue)
                throw new ArgumentException();
            graphics.DrawString(wordInfo.Word, font, Brushes.DarkSeaGreen, wordInfo.Rectangle.Value);
        }

        public bool Check(WordInfo wordInfo) => wordInfo.Word.Length < 4;
    }
}