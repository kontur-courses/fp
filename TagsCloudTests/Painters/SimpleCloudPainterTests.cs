using System.Drawing;
using FluentAssertions;
using Moq;
using TagsCloud.ColorGenerators;
using TagsCloud.ConsoleCommands;
using TagsCloud.Entities;
using TagsCloud.TagsCloudPainters;

namespace TagsCloudTests.Painters;

public class SimpleCloudPainterTests
{
    [SetUp]
    public void SetUp()
    {
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName.Replace("\\bin", "");
        var options = new Options() { OutputFile = Path.Combine(projectDirectory, "Painters", "images", FileName), Background = "Red"};
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
        var cloud = new Cloud(new List<Tag>(){},new Size(500, 500));
       painter.DrawCloud(cloud).IsSuccess.Should().BeFalse();
    }

    [Test]
    public void CloudLayouterDrawer_ShouldCreateImage()
    {
        var cloud = new Cloud(new List<Tag>()
        {
            new Tag(new Rectangle(0, 0, 5, 5), new Font("Arial", 10), "Hello")
        }, new Size(500, 500));
        var s =painter.DrawCloud(cloud);
        File.Exists(testFilePath).Should().BeTrue();
    }
}