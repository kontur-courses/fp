using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloudResult.TextProcessing;

namespace TagCloudResult
{
    public class TextProcessor_Should
    {
        private ITextReader reader;
        private TextProcessor textProcessor;
        private Settings settings;

        [SetUp]
        public void Setup()
        {
            reader = A.Fake<ITextReader>();
            settings = A.Fake<Settings>();
            A.CallTo(() => settings.TextPath).Returns("file");
            A.CallTo(() => settings.ExcludedWordsPath).Returns("ex_file");
            A.CallTo(() => reader.GetWordsFrom("file"))
                .Returns(Result.Ok(new List<string> { "words" } as IEnumerable<string>));
            A.CallTo(() => reader.GetWordsFrom("ex_file"))
                .Returns(Result.Ok(new List<string> { "ex_words" } as IEnumerable<string>));
            textProcessor = new TextProcessor(settings, reader);
        }

        [Test]
        public void ReturnFail_WhenErrorInWordsFile()
        {
            A.CallTo(() => reader.GetWordsFrom("file"))
                .Returns(Result.Fail<IEnumerable<string>>("No File"));

            var result = textProcessor.GetWordsFrequency();
            result.IsSuccess.Should().Be(false);
            result.Error.Should().Contain("No File");
        }
    }
}
