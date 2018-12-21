using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudResult
{
    internal class WordLayouter
    {
        private readonly Func<Word, Size> getWordSize;
        private readonly IRectangleLayout layout;
        private readonly List<Word> wordsToDraw;
        private List<Rectangle> coordinates;

        public WordLayouter(IWordStorage wordStorage, Func<Word, Size> getWordSize, IRectangleLayout layout)
        {
            wordsToDraw = wordStorage.ToIOrderedEnumerable().ToList();
            this.getWordSize = getWordSize;
            this.layout = layout;
        }

        public IEnumerable<ItemToDraw<Word>> GetItemsToDraws()
        {
            PlaceWords();

            return wordsToDraw
                .Select((word, i) => new ItemToDraw<Word>(
                    word, coordinates[i].X, 
                    coordinates[i].Y, 
                    coordinates[i].Width, 
                    coordinates[i].Height))
                .ToList();
        }

        private void PlaceWords()
        {
            var wordSizes = wordsToDraw
                .Select(w => getWordSize(w))
                .ToList();

            foreach (var size in wordSizes)
                layout.PutNextRectangle(size);

            coordinates = layout.GetCoordinatesToDraw().ToList();
        }
    }
}