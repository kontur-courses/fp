using System;
using System.Drawing;

namespace TagCloudVisualization
{
    public interface IWordDrawer
    {
        void DrawWord(Graphics graphics, ImageCreatingOptions options, WordInfo wordInfo, Font font);

        /// <summary>
        ///     Checks if this drawer is meant to draw this word
        /// </summary>
        bool Check(WordInfo wordInfo);
    }

    /// <summary>
    /// Purely for example
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
