using FluentAssertions;
using TagsCloudCore.WordProcessing.WordInput;

namespace TagsCloudCoreTests.WordProcessing.WordInput;

public class WordProviderTests
{
    [TestCaseSource(typeof(WordProviderTestCases), nameof(WordProviderTestCases.Providers))]
    public void GetWords_ReturnsFailedResult_OnIncorrectPath(IWordProvider wordProvider)
    {
        var result = wordProvider.GetWords("nonexistentresource");

        result.IsSuccess
            .Should()
            .BeFalse();
    }
}