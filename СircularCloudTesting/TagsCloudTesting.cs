using System;
using System.Drawing;
using System.Reflection;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;
using TagsCloudVisualization.App;
using TagsCloudVisualization.InterfacesForSettings;
using TagsCloudVisualization.TagsCloud;
using TagsCloudVisualization.TagsCloud.CircularCloud;

namespace СircularCloudTesting
{
    [TestFixture]
    public class TagsCloudTesting
    {

        private IContainer container;
        private Palette palette;
        private IImageSettings imageSettings;
        private IWordsSettings wordsSettings;
        private TagsCloudVisualizer visualizer;

        [SetUp]
        public void Init()
        {
            container = new DependencyBuilder().CreateContainer().Build();
            palette = container.Resolve<Palette>();
            palette.BackgroundColor = Color.White;
            palette.WordsColor = Color.Black;
            imageSettings = container.Resolve<IImageSettings>();
            imageSettings.Center = new Point(100, 100);
            imageSettings.ImageSize = new Size(200, 200);
            imageSettings.Font = new Font("Times New Roman", 5, FontStyle.Regular, GraphicsUnit.Pixel);
            wordsSettings = container.Resolve<IWordsSettings>();
            wordsSettings.PathToFile = $"{AppDomain.CurrentDomain.BaseDirectory}/TestingFiles/testText.txt";
            visualizer = container.Resolve<TagsCloudVisualizer>();
            var heightStretchFactor = typeof(TagsCloudVisualizer)
                .GetField("heightStretchFactor", BindingFlags.Instance | BindingFlags.NonPublic);
            heightStretchFactor.SetValue(visualizer, 1.5);
            var widthStretchFactor = typeof(TagsCloudVisualizer)
                .GetField("widthStretchFactor", BindingFlags.Instance | BindingFlags.NonPublic);
            widthStretchFactor.SetValue(visualizer, 1);

        }

        [Test]
        public void TagsCloud_Should_CorrectlyDisplayWordsWithCompression()
        {
            CircularCloudLayouter.IsCompressedCloud = true;
            var image = visualizer.DrawCircularCloud();
            var result = image.GetValueOrThrow().WithBitmapData(bmpData => bmpData.GetColorValues());
            var expectedImage = new Bitmap($"{AppDomain.CurrentDomain.BaseDirectory}/TestingFiles/CompressedTagCloud.png");
            var expectedResult = expectedImage.WithBitmapData(bmpData => bmpData.GetColorValues());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TagsCloud_Should_CorrectlyDisplayWordsWithoutCompression()
        {
            CircularCloudLayouter.IsCompressedCloud = false;
            var image = visualizer.DrawCircularCloud();
            var result = image.GetValueOrThrow().WithBitmapData(bmpData => bmpData.GetColorValues());
            var expectedImage = new Bitmap($"{AppDomain.CurrentDomain.BaseDirectory}/TestingFiles/NormalTagCloud.png");
            var expectedResult = expectedImage.WithBitmapData(bmpData => bmpData.GetColorValues());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void DrawCircularCloud_Should_Fail_WhenPathToFileIsMissing()
        {
            wordsSettings.PathToFile = null;
            var result = visualizer.DrawCircularCloud();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo("File not found.");
        }

        [Test]
        public void DrawCircularCloud_Should_Fail_WhenTagCloudIsLarge()
        {
            imageSettings.Font = new Font("Times New Roman", 20, FontStyle.Regular, GraphicsUnit.Pixel);
            var result = visualizer.DrawCircularCloud();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo("Tag cloud does not fit the image of the specified size.");
        }

        [Test]
        public void DrawCircularCloud_Should_Fail_WhenIncorrectImageSettingsSpecified()
        {
            imageSettings.Center = new Point(200, 300);
            imageSettings.ImageSize = new Size(200, 200);
            var result = visualizer.DrawCircularCloud();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo("Image settings are incorrect.");
        }

        [Test]
        public void TagsCloud_Should_Fail_WhenUnableToSaveFile()
        {
            var imageHolder = container.Resolve<PictureBoxImageHolder>();
            imageHolder.Image = null;
            imageHolder.OriginalImage = null;
            var result = imageHolder.SaveImage(@"fake\Cd.nmp");

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo("Image is missing.");
        }
        [Test]
        public void TagsCloud_Should_Fail_WhenInvalidPathSpecified()
        {
            var imageHolder = container.Resolve<PictureBoxImageHolder>();
            imageHolder.RecreateImage(new Bitmap(300, 300));
            var result = imageHolder.SaveImage(@"fake\Cd.nmp");

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo("Invalid path to file specified.");
        }
    }
}