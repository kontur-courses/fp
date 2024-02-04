using Moq;
using TagsCloudVisualization.Common;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.TagProviders;
using TagsCloudVisualization.TextReaders;
using TagsCloudVisualization.WordsAnalyzers;
using TagsCloudVisualization.WordsProcessors;
using TextReader = TagsCloudVisualization.TextReaders.TextReader;

namespace TagsCloudVisualizationTests;

public class TagProviderTests
{
    private ITagProvider tagProvider;
    private Mock<TextReader> textReader;
    private Mock<ITextReaderFactory> textReaderFactory;
    private Mock<IWordsProcessor> wordsProcessor;
    
    private const string DefaultText = "a a b c";
    private IEnumerable<Tag> defaultTags = new[]
    {
        new Tag("a", 1),
        new Tag("b", 0.5),
        new Tag("c", 0.5)
    };

    [SetUp]
    public void SetUp()
    {
        textReader = new Mock<TextReader>(new SourceSettings());
        textReaderFactory = new Mock<ITextReaderFactory>();
        wordsProcessor = new Mock<IWordsProcessor>();
        
        tagProvider = new TagProvider(textReaderFactory.Object, wordsProcessor.Object);

        textReader.Setup(x => x.GetText()).Returns(DefaultText);
        textReaderFactory.Setup(factory => factory.GetTextReader()).Returns(textReader.Object);
        wordsProcessor.Setup(x => x.Process(It.IsAny<IEnumerable<string>>()))
            .Returns(new Func<IEnumerable<string>, Result<IEnumerable<string>>>(x => x.AsResult())); ;
    }

    [Test]
    public void GetTags_ResultIsSuccess_WhenArgumentsOk()
    {
        tagProvider.GetTags().IsSuccess.Should().BeTrue();
    }

    [Test]
    public void GetTags_ReturnsCorrectTags_WhenArgumentsOk()
    {
        tagProvider.GetTags().GetValueOrThrow().Should().BeEquivalentTo(defaultTags);
    }

    [Test]
    public void GetTags_ResultIsFail_WhenTextReaderFactoryReturnsFail()
    {
        textReaderFactory.Setup(factory => factory.GetTextReader())
            .Returns(Result.Fail<TextReader>("factory failed"));

        tagProvider.GetTags().Should().Be(Result.Fail<IEnumerable<Tag>>("factory failed"));
    }
    
    [Test]
    public void GetTags_ResultIsFail_WhenTextReaderReturnsFail()
    {
        textReader.Setup(x => x.GetText())
            .Returns(Result.Fail<string>("textReader failed"));

        tagProvider.GetTags().Should().Be(Result.Fail<IEnumerable<Tag>>("textReader failed"));
    }
    
    [Test]
    public void GetTags_ResultIsFail_WhenWordsProcessorReturnsFail()
    {
        wordsProcessor.Setup(x => x.Process(It.IsAny<IEnumerable<string>>()))
            .Returns(Result.Fail<IEnumerable<string>>("wordsProcessor failed")); 

        tagProvider.GetTags().Should().Be(Result.Fail<IEnumerable<Tag>>("wordsProcessor failed"));
    }
    
    [Test]
    public void GetTags_UsingProcessedWords()
    {
        wordsProcessor.Setup(x => x.Process(It.IsAny<IEnumerable<string>>()))
            .Returns(new Func<IEnumerable<string>, Result<IEnumerable<string>>>(x => x.Where(w => w != "a").AsResult()));
        
        tagProvider.GetTags().GetValueOrThrow().Should().NotContain(tag => tag.Word == "a");
    }
}