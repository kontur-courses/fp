using System.Collections;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTests;

public class StringExtensionsTests
{
    [Test, TestCaseSource(nameof(GetAllWordsTestCases))]
    public void GetAllWords_ReturnsAllWordsFromText_Correctly(string text, IEnumerable<string> expected)
    {
        text.GetAllWords().Should().BeEquivalentTo(expected);
    }
    
    public static IEnumerable GetAllWordsTestCases
    {
        get
        {
            yield return new TestCaseData("", Array.Empty<string>());
            yield return new TestCaseData("a a b c", new[] {"a", "a", "b", "c"});
            yield return new TestCaseData("a]s \n asd", new[] {"a", "s", "asd"});
            yield return new TestCaseData(",,,,,.....\\\\@#$%&?", Array.Empty<string>());
            yield return new TestCaseData("0123456789", new[] {"0123456789"});
        }
    }
}
