using FluentAssertions;
using System.Text;
using TagsCloudContainer.FrequencyAnalyzers;

namespace TagsCloudTests
{
    internal class TextPreprocessorTests
    {
        private const string excludedWords = "stop\nword";
        private TextPreprocessing textPreprocessor;

        [SetUp]
        public void Setup()
        {
            textPreprocessor = new TextPreprocessing(CreateTempFile(excludedWords));
        }

        private string CreateTempFile(string content)
        {
            var tempFile = Path.GetTempFileName();
            using (var streamWriter = new StreamWriter(tempFile, false, Encoding.UTF8))
            {
                streamWriter.Write(content);
            }
            return tempFile;
        }

        [Test]
        public void Preprocess_ShouldExcludeCorrectWords()
        {
            var inputText = "stop\nword\nexample\none";
            var expectedWords = new[] { "example", "one" };

            var actualWords = new List<string>(textPreprocessor.Preprocess(inputText).GetValueOrDefault());

            actualWords.Should().BeEquivalentTo(expectedWords);
        }

        [Test]
        public void Preprocess_ShouldConvertToLowercase()
        {
            var inputText = "Example\nONE";
            var expectedWords = new[] { "example", "one" };

            var actualWords = textPreprocessor.Preprocess(inputText).GetValueOrDefault().ToList();

            actualWords.Should().BeEquivalentTo(expectedWords);
        }
    }
}
