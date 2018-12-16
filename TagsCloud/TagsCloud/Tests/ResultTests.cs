using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.CloudStructure;
using TagsCloud.ErrorHandling;
using TagsCloud.TagsCloudVisualization.ColorSchemes;
using TagsCloud.WordPrework;

namespace TagsCloud.Tests
{
    [TestFixture]
    public class ResultTests
    {
        private string filePath =
            Path.Combine(Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))),
                "Tests", "TestFile.txt");

        [TestCase(-1, 1, TestName = "Negative width")]
        [TestCase(1, -1, TestName = "Negative height")]
        [TestCase(0, 1, TestName = "Zero width")]
        [TestCase(1, 0, TestName = "Zero height")]
        public void PutNextRectangle_ReturnsResultWithError_OnWrongArguments(int width, int height)
        {
            var cloud = new PointCloudLayouter(new Point(0, 0), new SpiralPointGenerator(Math.PI / 16));
            var result = cloud.PutNextRectangle(new Size(width, height));
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void OptionsParse_ReturnsResultWithError_OnWrongArguments()
        {
            var options = Options.Parse(new[] { "--something" });
            options.IsSuccess.Should().BeFalse();
        }

        [TestCase(new object[]{ "--height", "-5" }, TestName = "Negative height")]
        [TestCase(new object[] { "--width", "-5" }, TestName = "Negative width")]
        [TestCase(new object[] { "--dangle", "-5" }, TestName = "Negative dangle")]
        [TestCase(new object[] { "--minFontSize", "-5" }, TestName = "Negative minFontSize")]
        [TestCase(new object[] { "--maxFontSize", "5" , "--minFontSize", "10"}, TestName = "MinFontSize is greater than maxFontSize")]
        [TestCase(new object[] { "--bgcolor", "-SOMECOLOR" }, TestName = "Unknown color name")]
        [TestCase(new object[] { "--font", "-SOMEFONT" }, TestName = "Unknown font name")]
        public void OptionsParse_ReturnsResultWithError_On(params string[] args)
        {
            var options = Options.Parse(args);
            options.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void DefineColor_ReturnsCorrectResult_OnCorrectFrequency()
        {
            var colorScheme = new RedColorScheme();
            var result = colorScheme.DefineColor(10);
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void DefineColor_ReturnsResultWithError_OnWrongFrequency()
        {
            var colorScheme = new RedColorScheme();
            var result = colorScheme.DefineColor(0);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void DefineColor_ReturnsResultWithCorrectError_OnWrongFrequency()
        {
            var colorScheme = new RedColorScheme();
            var result = colorScheme.DefineColor(0);
            result.Error.Should().Be("Wrong frequency '0'");
        }

        [Test]
        public void PutNextRectangle_ReturnsCorrectResult_OnCorrectRectangleSize()
        {
            var pointCloudLayouter = new PointCloudLayouter(new Point(), new SpiralPointGenerator(Math.PI/16));
            var result = pointCloudLayouter.PutNextRectangle(new Size(4,5));
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void PutNextRectangle_ReturnsResultWithError_OnWrongSize()
        {
            var pointCloudLayouter = new PointCloudLayouter(new Point(), new SpiralPointGenerator(Math.PI / 16));
            var result = pointCloudLayouter.PutNextRectangle(new Size(-4, 5));
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnsResultWithCorrectError_OnWrongSize()
        {
            var pointCloudLayouter = new PointCloudLayouter(new Point(), new SpiralPointGenerator(Math.PI / 16));
            var result = pointCloudLayouter.PutNextRectangle(new Size(-4, 5));
            result.Error.Should().Be("Wrong size parameters {Width=-4, Height=5}");
        }

        [Test]
        public void FileReader_ReturnsCorrectResult_OnCorrectFilePath()
        {
            var fileReader = new FileReader(filePath);
            fileReader.GetWords().All(w => w.IsSuccess).Should().BeTrue();
        }

        [Test]
        public void FileReader_ReturnsResultWithError_OnWrongFilePath()
        {
            var fileReader = new FileReader("Wrong path");
            fileReader.GetWords().Single().IsSuccess.Should().BeFalse();
        }

        [Test]
        public void FileReader_ReturnsResultWithCorrectError_OnWrongFilePath()
        {
            var fileReader = new FileReader("Wrong path");
            fileReader.GetWords().Single().Error.Should().Be("Exception in opening file 'Wrong path'");
        }

        [Test]
        public void WordAnalyzer_ReturnsResultWithError_OnWrongWords()
        {
            var wordAnalyzer = new WordAnalyzer(new List<Result<string>> { "汉", "字" });
            var result = wordAnalyzer.GetWordFrequency();
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetWordFrequency_ReturnsResultWithCorrectError_OnWrongWords()
        {
            var wordAnalyzer = new WordAnalyzer(new List<Result<string>> { "汉", "字" });
            var result = wordAnalyzer.GetWordFrequency();
            result.Error.Should().Be("Error in getting word frequency. " +
                    "Exception in getting the part of the speech of the word '汉'");
        }

        [Test]
        public void GetSpecificWordFrequency_ReturnsResultWithCorrectError_OnWrongWords()
        {
            var wordAnalyzer = new WordAnalyzer(new List<Result<string>> { "Hi", "汉", "字" });
            var result = wordAnalyzer.GetSpecificWordFrequency(new List<PartOfSpeech>
                {PartOfSpeech.Adjective, PartOfSpeech.Noun});
            result.Error.Should().Be("Error in getting specific word frequency. " +
                                     "Exception in getting the part of the speech of the word 'hi'");
        }

        [Test]
        public void WordAnalyzer_ReturnsCorrectResult_OnCorrectWords()
        {
            var wordAnalyzer = new WordAnalyzer(new List<Result<string>> { "Привет", "рад", "познакомиться" });
            var result = wordAnalyzer.GetWordFrequency();
            result.IsSuccess.Should().BeTrue();
        }
    }
}
