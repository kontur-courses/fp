using System.Collections.Generic;
using TagsCloudVisualization.Settings;
using TagsCloudVisualization.TagCloudLayouters;
using System.Linq;
using System.Drawing;
using System;

namespace TagsCloudVisualization.TagsCloudVisualization
{
    public class WordsCloudVisualization : ITagsCloudVisualization<Rectangle>
    {
        private readonly ImageSettings imageSettings;
        private readonly ILayouter circularCloudLayouter;

        public WordsCloudVisualization(ImageSettings imageSettings, ILayouter circularCloudLayouter)
        {
            this.imageSettings = imageSettings;
            this.circularCloudLayouter = circularCloudLayouter;
        }

        public void Draw(Dictionary<string, int> words)
        {
            var wordsRectangles = new Dictionary<string, Rectangle>();

            foreach (var wordInfo in words)
            {
                var size = imageSettings.TextRenderer.GetRectangleSize(imageSettings, wordInfo);
                var rectangle = circularCloudLayouter.PutNextRectangle(size);
                if (!rectangle.IsSuccess)
                    Environment.Exit(1);
                wordsRectangles.Add(wordInfo.Key, rectangle.Value);
            }

            var width = imageSettings.ImageSize.Width == 0 ?
                wordsRectangles.Max(elem => elem.Value.X + elem.Value.Width) : imageSettings.ImageSize.Width;
            var height = imageSettings.ImageSize.Height == 0 ?
                wordsRectangles.Max(elem => elem.Value.Y + elem.Value.Height) : imageSettings.ImageSize.Height;
            imageSettings.TextRenderer.PrintWords(width, height, wordsRectangles, imageSettings);
        }
    }
}
