using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NUnit.Framework;
using TagsCloud.Graphics;
using TagsCloud.Layout;

namespace TagsCloud.Tests
{
    public class GraphicsTest
    {
        [Test]
        public void SaveMap_CreatePng()
        {
            var words = new Dictionary<string, double>
            {
                {"car", 56},
                {"auto", 20},
                {"automobile", 24}
            };
            var center = new Point(0, 0);
            var width = 0.1;
            var step = 0.01;
            var layout =
                new TagCloudLayouter(new CircularCloudLayouter(center, new CircularSpiral(center, width, step)));
            var wordWithCoordinate = layout.GetLayout(words);
            var color = Color.Black;
            var imageSize = new Size(1000, 1000);
            var coordinatesAtImage = new CoordinatesAtImage(imageSize);
            var coordinates = coordinatesAtImage.GetCoordinates(wordWithCoordinate.Value);
            var fontFamily = new FontFamily("Consolas");
            var imageFormat = ImageFormat.Png;
            var imageName = Path.Combine(TestContext.CurrentContext.TestDirectory, "cloud.png");
            var imageSettings = new ImageSettings(imageSize, fontFamily, color, imageFormat, imageName);
            var graphics = new Picture(imageSettings);
            graphics.Save(coordinates.Value);
        }
    }
}

