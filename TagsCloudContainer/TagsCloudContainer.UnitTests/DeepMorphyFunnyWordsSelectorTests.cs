using DeepMorphy;

namespace TagsCloudContainer.UnitTests;

[TestFixture]
public class DeepMorphyFunnyWordsSelectorTests
{
    private readonly MorphAnalyzer morphAnalyzer = new();

    [Test]
    public void RecognizeFunnyCloudWords_Fail_AllWordsAreBoring()
    {
        var selector = new DeepMorphyFunnyWordsSelector(morphAnalyzer);
        var words = new[] { "в", "на", "ты" };

        var result = selector.RecognizeFunnyCloudWords(words);

        result.IsFailure.Should().BeTrue();
    }

    [Test]
    public void RecognizeFunnyCloudWords_Fail_NoOneWordIsSpecified()
    {
        var selector = new DeepMorphyFunnyWordsSelector(morphAnalyzer);
        var words = Array.Empty<string>();

        var result = selector.RecognizeFunnyCloudWords(words);

        result.IsFailure.Should().BeTrue();
    }

    [Test]
    public void RecognizeFunnyCloudWords_Success_SomeWordsIsFunny()
    {
        var selector = new DeepMorphyFunnyWordsSelector(morphAnalyzer);
        var words = new[] { "в", "на", "ты", "шлакоблокунь" };

        var result = selector.RecognizeFunnyCloudWords(words);

        result.IsSuccess.Should().BeTrue();
    }
}