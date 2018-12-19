using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using FakeItEasy;
using TagsCloudVisualization.App.Cloud.Words;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    class CloudVisualizationTests
    {
        private ConsoleApplication app;
        private IFileReader fileReader;
        private IVisualizer visualizer;
        private IWordPalette wordPalette;
        private IImageSaver imageSaver;

        [SetUp]
        public void SetUp()
        {
            var words = "aaa aaa aaa aaa bb bb bb bb cc cc";
            var graphicWords = new List<GraphicWord>()
            {
                new GraphicWord("aaa")
                {
                    Color = Color.Black, Font = new Font("Arial", 12), Rate = 4,
                    Rectangle = new Rectangle(10, 10, 10, 10)
                }
            };

            fileReader = A.Fake<IFileReader>();
            A.CallTo(() => fileReader.Read("path.txt")).WithAnyArguments().Returns(words);
            wordPalette = A.Fake<IWordPalette>();
            A.CallTo(() => wordPalette.ColorWords(graphicWords)).WithAnyArguments();
            visualizer = A.Fake<IVisualizer>();
            A.CallTo(() => visualizer.Render(graphicWords, 1000, 1000, wordPalette)).WithAnyArguments()
                .Returns(new Bitmap(1000, 1000));
            var sizeDefiner = A.Fake<ISizeDefiner>();
            var cloudLayouter = A.Fake<ICloudLayouter>();
            A.CallTo(() => cloudLayouter.Process(graphicWords, sizeDefiner, new Point(500, 500))).WithAnyArguments();
            var wordCounter = A.Fake<IWordCounter>();
            A.CallTo(() => wordCounter.Count(words)).WithAnyArguments().Returns(new List<GraphicWord>() {new GraphicWord("aaa")});
            imageSaver = A.Fake<IImageSaver>();
            A.CallTo(() => imageSaver.WriteToFile("asd", new Bitmap(10, 10))).WithAnyArguments().Returns(Result.Ok(new FileSaveResult()));

            app = new ConsoleApplication(fileReader, visualizer, wordPalette, sizeDefiner, cloudLayouter, wordCounter, imageSaver);
        }

        [Test]
        public void App_ShouldWorksCorrectly_OnCorrectArgs()
        {
            var result = app.GenerateImage(new[] {"--path", "test.txt"});
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void App_Fails_WithoutPath()
        {
            var result = app.GenerateImage(new string[0]);
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void App_FailsOn_NotPossitiveImageSize()
        {
            var result = app.GenerateImage(new[] { "--path", "test.txt", "--width", "0" });
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void App_FailsOn_InvalidFont()
        {
            var result = app.GenerateImage(new[] { "--path", "test.txt", "--font", "invalid"});
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void App_FailsOn_NotExistingFile()
        {
            A.CallTo(() => fileReader.Read("Not_existed_file")).Returns(Result.Fail<string>("not existed file"));
            var result = app.GenerateImage(new[] {"--path", "Not_existed_file"});
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void App_Fails_WhenCanNotRenderImage()
        {
            A.CallTo(() => visualizer.Render(null, 0, 0, wordPalette)).WithAnyArguments()
                .Returns(Result.Fail<Bitmap>("unable to render"));
            var result = app.GenerateImage(new[] { "--path", "test.txt" });
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void App_Fails_WhenCanNotSaveImage()
        {
            A.CallTo(() => imageSaver.WriteToFile("output.png", new Bitmap(10, 10))).WithAnyArguments()
                .Returns(Result.Fail<FileSaveResult>("unable to save image"));
            var result = app.GenerateImage(new[] { "--path", "test.txt" });
            Assert.IsFalse(result.IsSuccess);
        }
    }
}
