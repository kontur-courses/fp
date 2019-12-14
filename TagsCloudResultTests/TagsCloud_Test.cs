using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudResult;
using TagsCloudResult.ApplicationRunning;
using TagsCloudResult.CloudLayouters;
using TagsCloudResult.CloudLayouters.CircularCloudLayouter;
using TagsCloudResult.CloudVisualizers;
using TagsCloudResult.CloudVisualizers.BitmapMakers;
using TagsCloudResult.CloudVisualizers.ImageSaving;
using TagsCloudResult.TextParsing.CloudParsing;
using TagsCloudResult.TextParsing.CloudParsing.ParsingRules;
using TagsCloudResult.TextParsing.FileWordsParsers;

namespace TagsCloudResultTests
{
    [TestFixture]
    public class TagsCloud_Test
    {
        [SetUp]
        public void SetUp()
        {
            settings = new SettingsManager();
            var parser = new CloudWordsParser(() => settings.GetWordsParserSettings());
            var layouter = new CloudLayouter(() => settings.GetLayouterSettings());
            var visualizer = new CloudVisualizer(() => settings.GetVisualizerSettings());
            var saver = new ImageSaver(() => settings.GetImageSaverSettings());
            cloud = new TagsCloud(parser, layouter, visualizer, saver);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);
        }

        private SettingsManager settings;
        private TagsCloud cloud;
        private string testFilePath;

        private void MakeTestFile()
        {
            var currentPath = Directory.GetCurrentDirectory();
            var filename = "test.txt";
            var fullpath = Path.Combine(currentPath, filename);
            using (var fs = File.Create(fullpath))
            {
                var info = new UTF8Encoding(true).GetBytes("some\n\rtest\n\rwords\n\rtest\n\rwords\n\rtest\n\rtest");
                fs.Write(info, 0, info.Length);
            }

            testFilePath = fullpath;
        }

        [Test]
        public void GenerateTagCloud_Should_GenerateWithNoExceptions_When_WordsAreParsed()
        {
            MakeTestFile();
            settings.ConfigureWordsParserSettings(new TxtWordParser(), testFilePath, new DefaultParsingRule());
            cloud.ParseWords();
            var algorithm = new CircularCloudLayouter(new Point(0, 0), 0.1, 1);
            settings.ConfigureLayouterSettings(algorithm, 100, 0.1, 1);
            Following.Code(() => cloud.GenerateTagCloud()).Should().NotThrow("words are parsed");
        }

        [Test]
        public void GenerateTagCloud_Should_SaveCreateImage_When_AllStepsAreCorrect()
        {
            MakeTestFile();
            settings.ConfigureWordsParserSettings(new TxtWordParser(), testFilePath, new DefaultParsingRule());
            cloud.ParseWords();
            var algorithm = new CircularCloudLayouter(new Point(0, 0), 0.1, 1);
            settings.ConfigureLayouterSettings(algorithm, 100, 0.1, 1);
            cloud.GenerateTagCloud();
            var font = new Font("Arial", 16);
            settings.ConfigureVisualizerSettings(new Palette(), new DefaultBitmapMaker(), 700, 700, font);
            cloud.VisualizeCloud();
            var currentPath = Directory.GetCurrentDirectory();
            var filename = "test.jpg";
            var fullpath = Path.Combine(currentPath, filename);
            settings.ConfigureImageSaverSettings(ImageFormat.Jpeg, fullpath);
            cloud.SaveVisualized();
            File.Exists(fullpath).Should().BeTrue("all steps worked correctly");
            File.Delete(fullpath);
        }

        [Test]
        public void GenerateTagCloud_Should_SaveWithNoExceptions_When_CloudIsVisualized()
        {
            MakeTestFile();
            settings.ConfigureWordsParserSettings(new TxtWordParser(), testFilePath, new DefaultParsingRule());
            cloud.ParseWords();
            var algorithm = new CircularCloudLayouter(new Point(0, 0), 0.1, 1);
            settings.ConfigureLayouterSettings(algorithm, 100, 0.1, 1);
            cloud.GenerateTagCloud();
            var font = new Font("Arial", 16);
            settings.ConfigureVisualizerSettings(new Palette(), new DefaultBitmapMaker(), 700, 700, font);
            cloud.VisualizeCloud();
            var currentPath = Directory.GetCurrentDirectory();
            var filename = "test.jpg";
            var fullpath = Path.Combine(currentPath, filename);
            settings.ConfigureImageSaverSettings(ImageFormat.Jpeg, fullpath);
            Following.Code(() => cloud.SaveVisualized()).Should().NotThrow("all steps worked correctly");
            File.Delete(fullpath);
        }

        [Test]
        public void GenerateTagCloud_Should_ThrowInvalidOperationException_When_NoWordsParsed()
        {
            Following.Code(() => cloud.GenerateTagCloud()).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void GenerateTagCloud_Should_VisualizeCloudWithNoExceptions_When_CloudIsGenerated()
        {
            MakeTestFile();
            settings.ConfigureWordsParserSettings(new TxtWordParser(), testFilePath, new DefaultParsingRule());
            cloud.ParseWords();
            var algorithm = new CircularCloudLayouter(new Point(0, 0), 0.1, 1);
            settings.ConfigureLayouterSettings(algorithm, 100, 0.1, 1);
            cloud.GenerateTagCloud();
            var font = new Font("Arial", 16);
            settings.ConfigureVisualizerSettings(new Palette(), new DefaultBitmapMaker(), 700, 700, font);
            Following.Code(() => cloud.VisualizeCloud()).Should().NotThrow("cloud is successfully generated");
        }

        [Test]
        public void ParseWords_Should_ParseWithNoExceptions_When_FileExists()
        {
            MakeTestFile();
            settings.ConfigureWordsParserSettings(new TxtWordParser(), testFilePath, new DefaultParsingRule());
            Following.Code(() => cloud.ParseWords()).Should().NotThrow("file exists and is correct");
        }
    }
}