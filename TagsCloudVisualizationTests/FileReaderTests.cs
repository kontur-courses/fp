using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class FileReaderTests
    {
        private readonly TxtFileReader txtFileReader = new();

        [Test]
        public void FileReader_ShouldReadCorrectly_WhenOneWordOnLine()
        {
            const string filePath = "FileWithOneWordOnLine.txt";

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("One");
                writer.WriteLine("Word");
                writer.WriteLine("On");
                writer.WriteLine("Line");
            }

            var actual = txtFileReader.GetWordsFromFile(filePath, new[] { ' ' });

            actual.GetValueOrThrow().Should().BeEquivalentTo("One", "Word", "On", "Line");
        }

        [Test]
        public void FileReader_ShouldReadCorrectly_WhenSeveralWordsOnLine()
        {
            const string filePath = "FileWithSeveralWordsOnLine.txt";

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("One Two Three");
                writer.WriteLine("Words Where");
                writer.WriteLine("On Where");
                writer.WriteLine("Line Clear");
            }

            var actual = txtFileReader.GetWordsFromFile(filePath, new[] { ' ' });

            actual.GetValueOrThrow().Should()
                .BeEquivalentTo("One", "Two", "Three", "Words", "Where", "On", "Where", "Line", "Clear");
        }

        [Test]
        public void FileReader_ShouldReturnEmptyCollection_WhenFileIsEmpty()
        {
            const string filePath = "EmptyFile.txt";

            using (var _ = new StreamWriter(filePath))
            {
            }

            var actual = txtFileReader.GetWordsFromFile(filePath, new[] { ' ' });

            actual.GetValueOrThrow().Should().BeEmpty();
        }

        [Test]
        public void FileReader_ShouldReturnUnsuccessfulResult_WhenFileDoesNotExist()
        {
            var actual = txtFileReader.GetWordsFromFile("dasdadaasdsadasdsadasads.txt", new[] { ' ' });

            actual.IsSuccess.Should().BeFalse();
        }
    }
}