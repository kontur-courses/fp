using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TagsCloudVisualization.WordsProcessing;
using ResultOf;

namespace TagsCloudVisualization_Tests.WordProcessing
{
    [TestFixture]
    public class TextPreprocessor_Should
    {
        private TextPreprocessor preprocessor;
        private IWordsProvider wordsProvider;
        private IFilter filter;
        private IEnumerable<string> words;
        private IWordsChanger wordsChanger;

        [SetUp]
        public void SetUp()
        {
            wordsProvider = Substitute.For<IWordsProvider>();
            filter = Substitute.For<IFilter>();
            wordsChanger = Substitute.For<IWordsChanger>();
            words = new []{"A", "aa", "b"};
            wordsProvider.Provide().ReturnsForAnyArgs(Result.Ok(words.AsEnumerable()));
            filter.FilterWords(Arg.Any<IEnumerable<string>>()).ReturnsForAnyArgs(callInfo => Result.Ok(callInfo.Arg<IEnumerable<string>>()));
            wordsChanger.ChangeWords(Arg.Any<IEnumerable<string>>())
                .ReturnsForAnyArgs(callInfo => Result.Ok(callInfo.Arg<IEnumerable<string>>()?.Select(w => w.ToUpper())));
            preprocessor = new TextPreprocessor(wordsProvider, new []{filter}, new []{wordsChanger});
        }

        [Test]
        public void ProvidePreprocessedWords_WhenFiltering()
        {
            var expected = new[] { "aa" };
            filter.FilterWords(words).Returns(expected);
            wordsChanger.ChangeWords(expected).Returns(callInfo => Result.Ok(callInfo.Arg<IEnumerable<string>>()));
            var result = preprocessor.Provide();
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ProvidePreprocessedWords_WhenChanging()
        {
            var expected = new[] { "A", "AA", "B"};
            var result = preprocessor.Provide();
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ProvidePreprocessedWords_WhenFilteringAndChanging()
        {
            var expected = new[] { "A", "B"};
            filter.FilterWords(Arg.Any<IEnumerable<string>>()).ReturnsForAnyArgs(expected);
            var result = preprocessor.Provide();
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ProvidePreprocessedWords_WhenProviderFails()
        {
            wordsProvider.Provide().Returns(Result.Fail<IEnumerable<string>>("provider fail"));
            var result = preprocessor.Provide();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("provider fail");
        }

        [Test]
        public void ProvidePreprocessedWords_WhenFilterFails()
        {
            filter.FilterWords(Arg.Any<IEnumerable<string>>()).Returns(Result.Fail<IEnumerable<string>>("filter fail"));
            var result = preprocessor.Provide();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("filter fail");
        }

        [Test]
        public void ProvidePreprocessedWords_WhenChangerFails()
        {
            wordsChanger.ChangeWords(Arg.Any<IEnumerable<string>>()).Returns(Result.Fail<IEnumerable<string>>("changer fail"));
            var result = preprocessor.Provide();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("changer fail");
        }

        [Test]
        public void ProvidePreprocessedWords_WhenSeveralFails_OnlyFirstOneCounts()
        {
            wordsProvider.Provide().Returns(Result.Fail<IEnumerable<string>>("provider fail"));
            wordsChanger.ChangeWords(Arg.Any<IEnumerable<string>>()).Returns(Result.Fail<IEnumerable<string>>("changer fail"));
            var result = preprocessor.Provide();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("provider fail");
        }
    }
}
