using System;
using System.Drawing;
using CSharpFunctionalExtensions;

namespace TagCloudVisualization
{
    public class WordInfo
    {
        public WordInfo(string word, int count)
        {
            Word = word;
            Count = count;
        }

        public string Word { get; }

        public int Count { get; }

        public Maybe<Rectangle> Rectangle { get; private set; }

        public Maybe<float> Scale { get; private set; }

        public override string ToString() => $"{{{Word}, {Rectangle}, {Count}, {Scale}}}";

        public WordInfo With(Rectangle rectangle) => new WordInfo(Word, Count) {Rectangle = rectangle, Scale = Scale};

        public WordInfo With(float scale)
        {
            scale = Math.Abs(scale - 1) < 0.001 ? Word.Length : scale;
            return new WordInfo(Word, Count) {Scale = scale, Rectangle = Rectangle};
        }
    }
}
