using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudResult.CloudLayouters;
using TagsCloudResult.CloudVisualizers;
using TagsCloudResult.CloudVisualizers.ImageSaving;
using TagsCloudResult.TextParsing.CloudParsing;

namespace TagsCloudResult
{
    public class TagsCloud
    {
        private readonly ICloudLayouter cloudLayouter;
        private readonly ICloudVisualizer cloudVisualizer;
        private readonly IImageSaver imageSaver;
        private readonly ICloudWordsParser wordsParser;
        private List<CloudWord> parsedWords;
        private List<CloudVisualizationWord> visualizationWords;

        public TagsCloud(
            ICloudWordsParser wordsParser,
            ICloudLayouter cloudLayouter,
            ICloudVisualizer cloudVisualizer,
            IImageSaver imageSaver)
        {
            this.wordsParser = wordsParser;
            this.cloudLayouter = cloudLayouter;
            this.cloudVisualizer = cloudVisualizer;
            this.imageSaver = imageSaver;
        }

        public Bitmap VisualizedBitmap { get; private set; }

        public void ParseWords()
        {
            parsedWords = wordsParser.Parse().ToList();
        }

        public void GenerateTagCloud()
        {
            if (parsedWords is null) throw new InvalidOperationException("You should parse your words first!");
            visualizationWords = cloudLayouter.GetWords(parsedWords).ToList();
        }

        public void VisualizeCloud()
        {
            if (visualizationWords is null)
                GenerateTagCloud();
            VisualizedBitmap = cloudVisualizer.GetBitmap(visualizationWords);
        }

        public void SaveVisualized()
        {
            if (VisualizedBitmap is null)
                VisualizeCloud();
            imageSaver.Save(VisualizedBitmap);
        }
    }
}