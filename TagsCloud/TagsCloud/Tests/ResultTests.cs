using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.CloudStructure;
using TagsCloud.TagsCloudVisualization.ColorSchemes.SizeDefiners.ColorSchemes;
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

        private static WordAnalyzer CreateTestWordAnalyzer(IEnumerable<string> words, bool useInfinitiveForm = false)
        {
            var wordGetter = new SimpleWordGetter(words);
            return new WordAnalyzer(wordGetter, useInfinitiveForm);
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
            var wordAnalyzer = CreateTestWordAnalyzer(new List<string> { "汉", "字" });
            var result = wordAnalyzer.GetWordFrequency();
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetWordFrequency_ReturnsResultWithCorrectError_OnWrongWords()
        {
            var wordAnalyzer = CreateTestWordAnalyzer(new List<string> { "汉", "字" });
            var result = wordAnalyzer.GetWordFrequency();
            result.Error.Should().Be("Error in getting word frequency. " +
                    "Exception in getting the part of the speech of the word '汉'");
        }

        [Test]
        public void GetSpecificWordFrequency_ReturnsResultWithCorrectError_OnWrongWords()
        {
            var wordAnalyzer = CreateTestWordAnalyzer(new List<string> { "Hi", "汉", "字" });
            var result = wordAnalyzer.GetSpecificWordFrequency(new List<PartOfSpeech>
                {PartOfSpeech.Adjective, PartOfSpeech.Noun});
            result.Error.Should().Be("Error in getting specific word frequency. " +
                                     "Exception in getting the part of the speech of the word 'hi'");
        }

        [Test]
        public void WordAnalyzer_ReturnsCorrectResult_OnCorrectWords()
        {
            var wordAnalyzer = CreateTestWordAnalyzer(new List<string> { "Привет", "рад", "познакомиться" });
            var result = wordAnalyzer.GetWordFrequency();
            result.IsSuccess.Should().BeTrue();
        }
    }
}
