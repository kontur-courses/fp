using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.TextProcessing.Readers;
using TextReader = TagsCloudVisualization.TextProcessing.TextReader.TextReader;

namespace TagsCloudVisualizationTests.TextProcessingTests.TextReaderTests
{
    public class TextReaderTests
    {
        [SetUp]
        public void SetUp()
        {
            textReader = new TextReader(new IReader[]{new TxtReader(), new MSWordReader()});
        }

        private TextReader textReader;
        
        [Test]
        public void ReadAllText_ReturnsFailedResult_WhenTxtFileNotExists()
        {
            var result = textReader.ReadAllText("sadas");

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("File sadas does not exist");
        }
        
        [Test]
        public void ReadAllText_ReturnsFailedResult_WhenFileExtensionDoesNotSupport()
        {
            var path = @"..\..\..\TestTexts\file.pdf";

            var result = textReader.ReadAllText(path);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain(".pdf doesn't support");
        }

        [Test]
        public void ReadAllText_ContainWordsCountFromFile_WhenTxtFileContain4WordsWithoutForbiddenSigns()
        {
            var path = @"..\..\..\TestTexts\test1.txt";
            
            var result = textReader.ReadAllText(path);

            result.GetValueOrThrow().Split(' ').Length.Should().Be(4);
        }
        
        [Test]
        public void ReadAllText_ContainWordsFromTxtFile()
        {
            var path = @"..\..\..\TestTexts\test1.txt";
            
            var result = textReader.ReadAllText(path);

            result.GetValueOrThrow().Should().Be("hello world and Arina");
        }
        
        [Test]
        public void ReadAllText_ContainWordsFromDocxFile()
        {
            var path = @"..\..\..\TestTexts\test2.docx";
            
            var result = textReader.ReadAllText(path);

            result.GetValueOrThrow().Should().Be("Hello world with this beautiful text!");
        }
        
        [Test]
        public void ReadAllText_ContainWordsFromDocFile()
        {
            var path = @"..\..\..\TestTexts\test3.doc";
            
            var result = textReader.ReadAllText(path);

            result.GetValueOrThrow().Should().Be("Hello world with this beautiful text!");
        }
    }
}