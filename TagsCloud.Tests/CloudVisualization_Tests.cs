using System;
using System.Collections.Generic;
using System.Drawing;
using FakeItEasy;
using NUnit.Framework;
using TagsCloud.CloudLayouters;
using TagsCloud.Common;
using TagsCloud.Core;
using TagsCloud.FileReader;
using TagsCloud.Spirals;
using TagsCloud.Visualization;

namespace TagsCloud.Tests
{
    internal class CloudVisualization_Tests
    {
        private readonly List<(string, int)> words = new List<(string, int)>
        {
            ("первый", 8),
            ("второе", 7),
            ("третий", 6),
            ("четыре", 5),
            ("пять", 5),
            ("шесть", 5),
            ("семь", 4),
            ("восемь", 3),
            ("девять", 1)
        };

        private ICircularCloudLayouter cloud;
        private CloudVisualization cloudVisualizer;
        private FontSettings fontSettings;
        private Bitmap image;
        private IImageHolder imageHolder;

        private ISpiral spiral;
        private TagsHelper tagsHelper;

        [SetUp]
        public void SetUp()
        {
            image = new Bitmap(800, 800);
            fontSettings = new FontSettings {FontFamilyName = "Arial", FontSize = 20};

            imageHolder = A.Fake<IImageHolder>();
            A.CallTo(() => imageHolder.GetImageSize())
                .Returns(image.Size);
            A.CallTo(() => imageHolder.StartDrawing())
                .Returns(Graphics.FromImage(image));

            spiral = new ArchimedeanSpiral(new Point(image.Width / 2, image.Height / 2), 0.005);
            cloud = new CircularCloudLayouter(spiral);
            tagsHelper = new TagsHelper(new ReaderFactory());
            cloudVisualizer = new CloudVisualization(imageHolder, new Palette(), 
                fontSettings, new ColorAlgorithm(), tagsHelper);
        }

        [Test]
        public void Paint_ResultIsSucсess_WhenCloudFitIntoImageSize()
        {
            Assert.DoesNotThrow(() => cloudVisualizer.Paint(cloud, words));
        }

        [Test]
        public void Paint_ThrowException_WhenCloudDoesNotFitIntoImageSize()
        {
            fontSettings.FontSize = 50;
            Assert.Throws<InvalidOperationException>(() => cloudVisualizer.Paint(cloud, words));
        }
    }
}