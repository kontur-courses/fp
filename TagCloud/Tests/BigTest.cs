using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NUnit.Framework;
using TagsCloud.Graphics;
using TagsCloud.Layout;
using TagsCloud.Words;

namespace TagsCloud.Tests
{
    public class BigTest
    {
        [Test]
        public void DoSomething_WhenSomething()
        {
            var textPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Data/Text.docx");
            var parser = new WordsFromMicrosoftWord(textPath);
            var lowerParser = new LowerWords(parser);
            var boringWordsFromFile =
                new WordsFromFile(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data/BoringWords.txt"));
            var boringParser = new BoringWordsFilter(boringWordsFromFile.GetWords(), lowerParser.ToLower());
            var frequency = new FrequencyCollection();
            var frequencyNormalizedCollection = frequency.GetFrequencyCollection(boringParser.DeleteBoringWords().Value);
            var center = new Point(0, 0);
            var width = 0.1;
            var step = 0.01;
            var layout =
                new TagCloudLayouter(new CircularCloudLayouter(center, new CircularSpiral(center, width, step)));
            var wordWithCoordinate = layout.GetLayout(frequencyNormalizedCollection.Value);
            var color = Color.Black;
            var imageSize = new Size(1000, 1000);
            var coordinatesAtImage = new CoordinatesAtImage(imageSize);
            var coordinates = coordinatesAtImage.GetCoordinates(wordWithCoordinate.Value);
            var fontFamily = new FontFamily("Consolas");
            var imageFormat = ImageFormat.Png;
            var imageName = Path.Combine(TestContext.CurrentContext.TestDirectory, "BigCloud.png");
            var imageSettings = new ImageSettings(imageSize, fontFamily, color, imageFormat, imageName);
            var graphics = new Picture(imageSettings);
            graphics.Save(coordinates.Value);
        }
    }
}

