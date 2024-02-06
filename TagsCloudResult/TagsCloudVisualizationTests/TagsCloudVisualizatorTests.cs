using Moq;
using TagsCloudVisualization;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Common;
using TagsCloudVisualization.Common.ResultOf;
using TagsCloudVisualization.TagProviders;
using TagsCloudVisualization.TextReaders;
using TagsCloudVisualization.WordsProcessors;
using TextReader = System.IO.TextReader;

namespace TagsCloudVisualizationTests;

public class TagsCloudVisualizatorTests
{
    private TagsCloudVisualizator tagsCloudVisualizator;
    private Mock<IImageHolder> imageHolder;
    private Mock<ITagsCloudLayouter> layouter;
    private Mock<ITagProvider> tagProvider;
    private TagsSettings tagsSettings;
    
    private readonly IEnumerable<Tag> defaultTags = new[]
    {
        new Tag("a", 1),
        new Tag("b", 0.5),
        new Tag("c", 0.5)
    };

    [SetUp]
    public void SetUp()
    {
        var rectangle = new Rectangle(5, 5, 5, 5);
        
        tagsSettings = new TagsSettings();
        tagProvider = new Mock<ITagProvider>();
        layouter = new Mock<ITagsCloudLayouter>();
        imageHolder = new Mock<IImageHolder>();
        tagsCloudVisualizator =
            new TagsCloudVisualizator(layouter.Object, imageHolder.Object, tagProvider.Object, tagsSettings);

        tagProvider.Setup(x => x.GetTags()).Returns(defaultTags.AsResult);
        layouter.Setup(x => x.PutNextRectangle(It.IsAny<Size>())).Returns(rectangle);
        imageHolder.Setup(x => x.StartDrawing()).Returns(Graphics.FromImage(new Bitmap(100, 100)));
        imageHolder.Setup(x => x.UpdateUi()).Returns(Result.Ok);
    }

    [Test]
    public void DrawTagsCloud_ResultIsSuccess_WhenArgumentsOk()
    {
        tagsCloudVisualizator.DrawTagsCloud().Should().Be(Result.Ok());
    }

    [Test]
    public void DrawTagsCloud_ResultIsFail_WhenTagProviderFails()
    {
        tagProvider.Setup(x => x.GetTags()).Returns(Result.Fail<IEnumerable<Tag>>("tagprovider failed"));
        
        tagsCloudVisualizator.DrawTagsCloud().Should().Be(Result.Fail<None>("tagprovider failed"));
    }
    
    [Test]
    public void DrawTagsCloud_ResultIsFail_WhenTagCloudLayouterFails()
    {
        layouter.Setup(x => x.PutNextRectangle(It.IsAny<Size>())).Returns(Result.Fail<Rectangle>("layouter failed"));
        
        tagsCloudVisualizator.DrawTagsCloud().Should().Be(Result.Fail<None>("layouter failed"));
    }
    
    [Test]
    public void DrawTagsCloud_ResultIsFail_WhenImageHolderFailsStartDrawing()
    {
        imageHolder.Setup(x => x.StartDrawing()).Returns(Result.Fail<Graphics>("can't provide graphics"));
        
        tagsCloudVisualizator.DrawTagsCloud().Should().Be(Result.Fail<None>("can't provide graphics"));
    }
    
    [Test]
    public void DrawTagsCloud_ResultIsFail_WhenImageHolderFailsUpdateUI()
    {
        imageHolder.Setup(x => x.UpdateUi()).Returns(Result.Fail<None>("can't update ui"));
        
        tagsCloudVisualizator.DrawTagsCloud().Should().Be(Result.Fail<None>("can't update ui"));
    }
}
