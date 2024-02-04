using System.Drawing;
using FluentAssertions;
using Moq;
using TagsCloud.ColorGenerators;
using TagsCloud.ConsoleCommands;
using TagsCloud.Entities;
using TagsCloud.Layouters;
using TagsCloud.Result;
using TagsCloud.TagsCloudPainters;

namespace TagsCloudTests.Painters;

public class SimpleCloudPainterTests
{
    [SetUp]
    public void SetUp()
    {
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName.Replace("\\bin", "");
        var options = new Options() { OutputFile = Path.Combine(projectDirectory, "Painters", "images", FileName), Background = "Empty"};
        testFilePath = options.OutputFile;
        var color = new Mock<IColorGenerator>();
        color.Setup(c => c.GetTagColor(It.IsAny<Tag>())).Returns(new Result<Color>(null,Color.Black));
        painter = new SimplePainter(color.Object, options);
        if (File.Exists(testFilePath)) File.Delete(testFilePath);
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(testFilePath)) File.Delete(testFilePath);
    }

    private IPainter painter;
    private const string FileName = "testcreation.png";
    private string testFilePath;

    [Test]
    public void CloudLayouterDrawerConstructor_ThrowsInvalidOperationException_WhenRectanglesLengthIsZero()
    {
        var layouter = new Mock<ILayouter>();
        layouter.Setup(l => l.GetTagsCollection()).Returns(new Result<IEnumerable<Tag>>("",new List<Tag>(){}));
        layouter.Setup(l => l.GetImageSize()).Returns(new Result<Size>(null,new Size(500, 500)));
        Assert.Throws<InvalidOperationException>(() => painter.DrawCloud(layouter.Object.GetTagsCollection().GetValueOrThrow(),layouter.Object.GetImageSize().GetValueOrThrow()));
    }

    [Test]
    public void CloudLayouterDrawer_ShouldCreateImage()
    {
        var layouter = new Mock<ILayouter>();
        layouter.Setup(l => l.GetTagsCollection()).Returns(new Result<IEnumerable<Tag>>(null, new List<Tag>()
        {
            new Tag(new Rectangle(0, 0, 5, 5), new Font("Arial", 10), "Hello")
        }));
        layouter.Setup(l => l.GetImageSize()).Returns(new Result<Size>(null,new Size(500, 500)));
        painter.DrawCloud(layouter.Object.GetTagsCollection().GetValueOrThrow(),layouter.Object.GetImageSize().GetValueOrThrow());
        File.Exists(testFilePath).Should().BeTrue();
    }
}