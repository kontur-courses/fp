using System.Drawing;
using FluentAssertions;
using Moq;
using TagsCloud.ColorGenerators;
using TagsCloud.ConsoleOptions;
using TagsCloud.Entities;
using TagsCloud.Options;
using TagsCloud.TagsCloudPainters;

namespace TagsCloudTests.Painters;

public class SimpleCloudPainterTests
{
    private static string projectDirectory =
        Directory.GetParent(Environment.CurrentDirectory).Parent.FullName.Replace("\\bin", "");

    private string filename = Path.Combine(projectDirectory, "Painters", "images", FileName);
    private SimplePainter painter;
    private const string FileName = "testcreation.png";

    [SetUp]
    public void SetUp()
    {
        var options = new LayouterOptions() { OutputFile = filename, BackgroundColor = Color.Empty, ImageSize = new Size(500,500)};
        var color = new Mock<IColorGenerator>();
        color.Setup(c => c.GetTagColor(It.IsAny<Tag>())).Returns(Color.Black);
        painter = new SimplePainter(color.Object, options);
        if (File.Exists(filename)) File.Delete(filename);
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(filename)) File.Delete(filename);
    }

    [Test]
    public void SimplePainerDrawCloud_ShouldReturnErrorResult_WhenInputTagCollectionIsEmpty()
    {
        var cloud = new Cloud(new List<Tag>() { }, new Size(500, 500));
        painter.DrawCloud(cloud).IsSuccess.Should().BeFalse();
    }

    [Test]
    public void SimplePainerDrawCloud_ShouldReturnErrorResult_WhenCantFitCloudInInputSize()
    {
        var tagsList = new List<Tag>(){new Tag(new Rectangle(0, 0, 50, 50), new Font("Arial", 5), "apple")};
        var bigSize = new Size(1000, 1000);
        var cloud = new Cloud(tagsList, bigSize);

        painter.DrawCloud(cloud).IsSuccess.Should().BeFalse();

    }

    [Test]
    public void CloudLayouterDrawer_ShouldCreateImage()
    {
        var cloud = new Cloud(new List<Tag>()
        {
            new Tag(new Rectangle(0, 0, 5, 5), new Font("Arial", 10), "Hello")
        }, new Size(100, 100));
        var s = painter.DrawCloud(cloud);
        File.Exists(filename).Should().BeTrue();
    }
}