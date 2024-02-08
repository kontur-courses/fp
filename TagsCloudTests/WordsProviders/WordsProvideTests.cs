using FluentAssertions;
using Moq;
using TagsCloud.ConsoleOptions;
using TagsCloud.Options;
using TagsCloud.WordsProviders;
using TagsCloud.WordValidators;

namespace TagsCloudTests.WordsProviders;

[TestFixture]
public class WordsProviderTests
{
    private Mock<IWordValidator> validator;

    [SetUp]
    public void Setup()
    {
        validator = new Mock<IWordValidator>();
        validator.Setup(v => v.IsWordValid(It.IsAny<string>())).Returns(new Result<bool>(null,true));
    }


    [Test]
    public void WordsProvider_ShouldThrowFileNotFoundException_WhenReadTextFromIncorrectFilePath()
    {
        var options = new LayouterOptions(){InputFile = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "input.txt")};
        options.InputFile =
            Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "input.txt");
        var textReader = new WordsProvider(validator.Object, options);
        textReader.GetWords().IsSuccess.Should().BeFalse();
    }
    
    [Test]
    public void WordsProvider_ShouldThrowFileNotFoundException_WhenNoTextInFile()
    {
        var options = new LayouterOptions(){InputFile = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName.Replace("\\bin",""), "inputempty2.txt")};
        var textReader = new WordsProvider(validator.Object, options);
        textReader.GetWords().IsSuccess.Should().BeFalse();
    }

    [Test]
    public void WordsProvider_ShouldReadTextFromFile()
    {
        var options = new LayouterOptions();
        options.InputFile =
            Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName.Replace("\\bin", ""),
                "WordsProviders", "input.txt");
        var words = new WordsProvider(validator.Object, options).GetWords().GetValueOrThrow();
        var dict = new Dictionary<string, int>()
        {
            { "ренат", 3 },
            { "привет", 4 },
            { "дом", 1 },
            { "стол", 2 }
        };
        words.Should().BeEquivalentTo(dict);
    }
}