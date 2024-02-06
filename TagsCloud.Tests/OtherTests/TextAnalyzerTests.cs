using NUnit.Framework;
using System.Collections;
using TagsCloud.TextAnalysisTools;
using TagsCloudVisualization;

namespace TagsCloud.Tests.OtherTests;

[TestFixture]
[TestOf(nameof(TextAnalyzer))]
public class TextAnalyzerTests
{
    [TestCaseSource(nameof(TestGroups))]
    public bool TextAnalyzer_Should_DistinguishRussianAndOtherWords(WordTagGroup group)
    {
        TextAnalyzer.FillWithAnalysis(new HashSet<WordTagGroup> { group });
        return group.WordInfo.IsRussian;
    }

    private static IEnumerable TestGroups()
    {
        yield return new TestCaseData(new WordTagGroup("Apple", 1)).Returns(false);
        yield return new TestCaseData(new WordTagGroup("Игра", 1)).Returns(true);
        yield return new TestCaseData(new WordTagGroup("BMW", 1)).Returns(false);
        yield return new TestCaseData(new WordTagGroup("Богатырь", 1)).Returns(true);
        yield return new TestCaseData(new WordTagGroup("Математика", 1)).Returns(true);
        yield return new TestCaseData(new WordTagGroup("C#", 1)).Returns(false);
        yield return new TestCaseData(new WordTagGroup("Fibonacci", 1)).Returns(false);
    }
}