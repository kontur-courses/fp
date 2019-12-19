using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Extensions;

namespace TagsCloudContainer.Algorithm.SizeProviding
{
    public class CorrespondingToWeightSizeProvider : IWordSizeProvider
    {
        private readonly int scaleFactor;

        public CorrespondingToWeightSizeProvider(int scaleFactor)
        {
            this.scaleFactor = scaleFactor;
        }

        public CorrespondingToWeightSizeProvider() : this(4) { }

        public IEnumerable<Word> SetWordsSizes(IEnumerable<Word> words, Size pictureSize)
        {
            var lastWordSize = Size.Empty;
            var lastWordWeight = 0;
            var wordsList = words.ToList();
            foreach (var word in wordsList.OrderBy(w => w.Weight))
            {
                var area = lastWordSize == Size.Empty
                    ? GetFirstWordSizeArea(wordsList.Count, pictureSize)
                    : GetNextWordSizeArea(word.Weight, lastWordSize.GetArea(), lastWordWeight);

                word.Size = GetWordSizeByArea(area);
                CheckIfWordSizeIsCorrect(word.Size);
                yield return word;
                lastWordSize = word.Size;
                lastWordWeight = word.Weight;
            }
        }

        private int GetFirstWordSizeArea(int wordsCount, Size pictureSize)
        {
            return pictureSize.GetArea() / (wordsCount * scaleFactor);
        }

        private int GetNextWordSizeArea(int weight, int lastWordSizeArea, int lastWordWeight)
        {
            return lastWordSizeArea * weight / lastWordWeight;
        }

        private Size GetWordSizeByArea(int area)
        {
            var height = (int)Math.Sqrt((double)area / 2);
            var width = height * 2;
            return new Size(width, height);
        }

        private void CheckIfWordSizeIsCorrect(Size wordSize)
        {
            if (wordSize.Height == 0 || wordSize.Width == 0)
            {
                throw new ArgumentException($"Can't provide sizes for words, maybe picture size was too small");
            }
        }
    }
}