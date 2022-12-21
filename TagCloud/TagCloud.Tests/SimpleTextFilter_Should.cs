using FluentAssertions;
using TagCloud.Common.TextFilter;

namespace TagCloud.Tests;

[TestFixture]
public class SimpleTextFilter_Should
{
    private ITextFilter filter;

    [SetUp]
    public void SetUp()
    {
        filter = new SimpleTextFilter();
    }

    [Test]
    public void FilterAllWords_ShouldWorkCorrect_WithEmptyCollection()
    {
        var words = filter.FilterAllWords(new List<string>(), 0);
        words.IsSuccess.Should().BeFalse();
        words.Error.Should().Be("Filtered 0 words inside text");
    }

    [Test]
    public void FilterAllWords_ShouldWorkCorrect_WithEmptyStringInsideCollection()
    {
        var words = filter.FilterAllWords(new List<string> { "" }, 0);
        words.IsSuccess.Should().BeFalse();
        words.Error.Should().Be("Filtered 0 words inside text");
    }

    [TestCase("строка без символов", 3)]
    [TestCase("однослово", 1)]
    public void FilterAllWords_ShouldWorkCorrect_SimpleLines(string line, int wordsCount)
    {
        var words = filter.FilterAllWords(new List<string> { line }, 0);
        words.GetValueOrThrow().Count().Should().Be(wordsCount);
    }

    [Test]
    public void FilterAllWords_ShouldWorkCorrect_WithManyLines()
    {
        var words = filter.FilterAllWords(new List<string> { "однослово", "два слова", "три слова да" }, 0);
        words.GetValueOrThrow().Count().Should().Be(6);
    }

    [TestCase("Простое предложение с точкой в конце.", 6, "конце")]
    [TestCase("Одно предложение, разделенное запятой", 4, "предложение")]
    public void FilterAllWords_ShouldWorkCorrect_DefaultSentences(string line, int wordsCount, string lastWord)
    {
        var words = filter.FilterAllWords(new List<string> { line }, 0).GetValueOrThrow().ToList();
        words.Count.Should().Be(wordsCount);
        words.Should().Contain(lastWord);
    }

    [TestCase("МНОГО СЛОВ В ВЕРХНЕМ РЕГИСТРЕ")]
    [TestCase("ОдноСлово")]
    public void FilterAllWords_Should_TranslateToLowerCase(string line)
    {
        var words = filter.FilterAllWords(new List<string> { line }, 0).GetValueOrThrow().ToList();
        words.Any(word => word != word.ToLower()).Should().BeFalse();
    }

    [TestCase("б у к в ы слово", 1, 1)]
    [TestCase("что где когда", 3, 1)]
    [TestCase("три,два,один", 3, 1)]
    public void FilterAllWords_Should_ExcludeBoringWords(string line, int boringWordsBorder, int wordsCount)
    {
        var words = filter.FilterAllWords(new List<string> { line }, boringWordsBorder).GetValueOrThrow().ToList();
        words.Count.Should().Be(wordsCount);
    }

    [TestCase(
        "Кринж — это в молодежном сленге дернуться от страха, испытывать стыд или негодование из-за действий другого человека. В том числе это может означать мерзкое, жуткое, ужасное нечто. Термин может использоваться в адрес действия, существа, явления или события. Современное понятие имеет отношение к англицизмам — терминам, взятым из английского лексикона. Кринж пишется на английском как «cringe», также под данным словом подразумевают глаголы «коробить» или «передергивать».",
        3, 46)]
    public void FilterAllWords_Should_WorkWithHardCases(string line, int boringWordsBorder, int wordsCount)
    {
        var words = filter.FilterAllWords(new List<string> { line }, boringWordsBorder).GetValueOrThrow().ToList();
        words.Count.Should().Be(wordsCount);
    }
}