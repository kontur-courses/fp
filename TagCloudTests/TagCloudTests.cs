using System;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagCloudTests
{
    [TestFixture]
    public class TagCloudTests
    {
        private Random random = new Random();
        private TagCloudFactory factory = new TagCloudFactory();
        private TagCloud tagCloud;
        private string sourceFile = "testIn.txt";
        private const string DefaultContent = "Tag tag  tag cloud cloud test";
        private string resultFile;
        
        [SetUp]
        public void SetUp()
        {
            sourceFile = random.Next() + ".txt";
            resultFile = random.Next() + ".png";
            tagCloud = factory.CreateInstance(false, "sorted").Value;
            CreateFile(sourceFile, DefaultContent);
        }

        private void CreateFile(string path, string content)
        {
            using var file = File.CreateText(path);
            file.Write(content);
        }

        [TearDown]
        public void DeleteFiles()
        {
            try
            {
                File.Delete(sourceFile);
                File.Delete(resultFile);
            }
            catch
            {
                // ignored
            }
        }

        [TestCase(-100, 100)]
        [TestCase(100, 0)]
        [TestCase(-100, -100)]
        public void TagCloud_NonPositiveResolution_Fail(int width, int height)
        {
            var result = tagCloud.CreateTagCloudFromFile(sourceFile, resultFile, SystemFonts.DefaultFont,
                new Color(), 100, new Size(width, height));
            result.Error.Should().Be("Resolution must be positive");
        }
        
        [TestCase("Comic Sans")]
        [TestCase("qwerty")]
        [TestCase("default")]
        public void TagCloud_UnknownFont_Fail(string fontName)
        {
            var result = tagCloud.CreateTagCloudFromFile(sourceFile, resultFile, fontName,
                "Red", 100, 100, 100);
            result.Error.Should().MatchRegex("Unknown Font *");
        }
        
        [TestCase("red")]
        [TestCase("graybol")]
        [TestCase("default")]
        public void TagCloud_UnknownColor_Fail(string colorName)
        {
            var result = tagCloud.CreateTagCloudFromFile(sourceFile, resultFile, "Comic Sans MS",
                colorName, 100, 100, 100);
            result.Error.Should().MatchRegex("Unknown color *");
        }
        
        [Test]
        public void TagCloud_SourceNotExist_Fail()
        {
            DeleteFiles();
            var result = tagCloud.CreateTagCloudFromFile(sourceFile, resultFile, "Comic Sans MS",
                "Red", 100, 100, 100);
            result.Error.Should().Be("Source file not found");
        }
        
        [TestCase("doc")]
        [TestCase("docx")]
        [TestCase("pdf")]
        [TestCase("png")]
        [TestCase("xyz")]
        public void TagCloud_SourceWrongFormat_Fail(string extension)
        {
            DeleteFiles();
            sourceFile = random.Next() + "." + extension;
            CreateFile(sourceFile, DefaultContent);
            var result = tagCloud.CreateTagCloudFromFile(sourceFile, resultFile, "Comic Sans MS",
                "Red", 100, 100, 100);
            result.Error.Should().Be("Unknown source file format.");
        }
        
        [TestCase("txt")]
        public void TagCloud_SourceRightFormat_Success(string extension)
        {
            DeleteFiles();
            sourceFile = random.Next() + "." + extension;
            CreateFile(sourceFile, DefaultContent);
            var result = tagCloud.CreateTagCloudFromFile(sourceFile, resultFile, "Comic Sans MS",
                "Red", 100, 100, 100);
            result.IsSuccess.Should().BeTrue();
        }
        
        [Test]
        public void TagCloud_EmptySource_Fail()
        {
            DeleteFiles();
            CreateFile(sourceFile, "");
            var result = tagCloud.CreateTagCloudFromFile(sourceFile, resultFile, "Comic Sans MS",
                "Red", 100, 100, 100);
            result.Error.Should().Be("Zero tags found");
        }
        
        [TestCase("doc")]
        [TestCase("docx")]
        [TestCase("pdf")]
        [TestCase("sas")]
        [TestCase("xyz")]
        public void TagCloud_ResultWrongFormat_Fail(string extension)
        {
            resultFile = random.Next() + "."  + extension;
            var result = tagCloud.CreateTagCloudFromFile(sourceFile, resultFile, "Comic Sans MS",
                "Red", 100, 100, 100);
            result.Error.Should().Be("Unknown image format");
        }
        
        [TestCase("png")]
        [TestCase("jpg")]
        [TestCase("jpeg")]
        [TestCase("tiff")]
        [TestCase("bmp")]
        public void TagCloud_ResultRightFormat_Success(string extension)
        {
            resultFile = random.Next() + "."  + extension;
            var result = tagCloud.CreateTagCloudFromFile(sourceFile, resultFile, "Comic Sans MS",
                "Red", 100, 100, 100);
            result.IsSuccess.Should().BeTrue();
        }
    }
}