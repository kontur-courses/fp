using FluentAssertions;
using NSubstitute;
using TagsCloudCore;
using TagsCloudCore.WordProcessing.WordFiltering;
using TagsCloudCore.WordProcessing.WordInput;

namespace TagsCloudCoreTests.WordProcessing.WordFiltering;

public class DefaultWordFilterTests
{
    [Test]
    public void FilterWords_ReturnsCorrectResult_OnCorrectInputData()
    {
        var wordProvider = Substitute.For<IWordProvider>();
        wordProvider.GetWords(Arg.Any<string>()).Returns(new[] {"word1", "word2", "123"});

        var filter = new DefaultWordFilter(wordProvider, "");
        var filtered = filter.FilterWords(new[] {"word1", "1234", "word2", "a", "another", "test"});

        CollectionAssert.AreEqual(new[] {"1234", "a", "another", "test"}, filtered.Value);
    }

    [Test]
    public void FilterWords_FailedResultReturnsCorrectErrorMessage_WhenCannotReadFromFilterFile()
    {
        var wordProvider = Substitute.For<IWordProvider>();
        wordProvider.GetWords(Arg.Any<string>()).Returns(Result.Fail<string[]>("123"));
        var filter = new DefaultWordFilter(wordProvider, "");

        var result = filter.FilterWords(Array.Empty<string>());

        result.Error
            .Should()
            .Contain("Failed to read from the word filter file");
    }
}