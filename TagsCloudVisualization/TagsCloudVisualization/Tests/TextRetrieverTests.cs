using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Logic;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class TextRetrieverTests
    {
        [Test]
        public void TextRetriever_ThrowsNullArgumentException_WhenPathIsNull()
        {
            var result = TextRetriever.RetrieveTextFromFile(null);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void TextRetriever_ThrowsArgumentException_WhenFileDoesNotExists()
        {
            var result = TextRetriever.RetrieveTextFromFile("nonexistingpath");
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void TextRetriever_ReturnsCorrectText_WhenFileIsTxt()
        {
            var textPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Tests", "TestTexts", "animals.txt");
            var expectedText = File.ReadAllText(textPath, Encoding.UTF8);
            TextRetriever.RetrieveTextFromFile(textPath).GetValueOrThrow().Should().BeEquivalentTo(expectedText);
        }
    }
}