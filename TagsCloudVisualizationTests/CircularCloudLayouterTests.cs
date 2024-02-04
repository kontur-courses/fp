using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.PointCreators;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private Point center = new(250, 250);
    private ImageSettings imageSettings = new ImageSettings(500, 500);
    private SpiralSettings spiralSettings = new SpiralSettings(0.05, 0.1);
    private CircularCloudLayouter sut;
    private IPointCreator pointCreator;

    [SetUp]
    public void SetUp()
    {
        pointCreator = new Spiral(imageSettings, spiralSettings);
        sut = new CircularCloudLayouter(pointCreator);
    }

    [Test]
    public void Constructor_NotTrows()
    {
        var action = () => new CircularCloudLayouter(pointCreator);
        action.Should().NotThrow();
    }

    [TestCase(-1, 1, TestName = "PutNextRectangle_WidthNotPositive_ReturnsFalse")]
    [TestCase(1, -1, TestName = "PutNextRectangle_HeightNotPositive_ReturnsFalse")]
    public void PutNextRectangle_IncorrectSize_ReturnsFalse(int rectangleWidth, int rectangleHeight)
    {
        var result = sut.PutNextRectangle(new Size(rectangleWidth, rectangleHeight));
        result.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_WithCorrectSize_ReturnsCorrectRectangle()
    {
        var rectangle = sut.PutNextRectangle(new Size(15, 15));
        rectangle.GetValueOrThrow().Size.Should().BeEquivalentTo(new Size(15, 15));
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_ReturnsRectangleWithCenterInLayoutCenter()
    {
        var rectangle = sut.PutNextRectangle(new Size(15, 15));
        var expectedRectangleCenter = new Point(rectangle.GetValueOrThrow().Left + 
            rectangle.GetValueOrThrow().Width / 2, 
            rectangle.GetValueOrThrow().Top + rectangle.GetValueOrThrow().Height / 2);

        expectedRectangleCenter.Should().BeEquivalentTo(center);
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_ReturnsSecondRectangleWithCenterNotInLayoutCenter()
    {
        sut.PutNextRectangle(new Size(15, 15));
        var secondRectangle = sut.PutNextRectangle(new Size(10, 10));
        var expectedRectangleCenter = new Point(secondRectangle.GetValueOrThrow().Left + secondRectangle.GetValueOrThrow().Width / 2,
            secondRectangle.GetValueOrThrow().Top + secondRectangle.GetValueOrThrow().Height / 2);

        expectedRectangleCenter.Should().NotBeEquivalentTo(center);
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_ReturnsTwoRectangles()
    {
        sut.PutNextRectangle(new Size(15, 15));
        sut.PutNextRectangle(new Size(10, 10));

        sut.Rectangles.Count.Should().Be(2);
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_ReturnsTwoNotIntersectedRectangles()
    {
        var firstRectangle = sut.PutNextRectangle(new Size(15, 15));
        var secondRectangle = sut.PutNextRectangle(new Size(10, 10));
        var isIntersected = firstRectangle.GetValueOrThrow().IntersectsWith(secondRectangle.GetValueOrThrow());

        isIntersected.Should().BeFalse();
    }

    [TestCase(10, TestName = "PutNextRectangle_10Rectangles_RectanglesWithNoIntersects")]
    [TestCase(100, TestName = "PutNextRectangle_100Rectangles_RectanglesWithNoIntersects")]
    [TestCase(200, TestName = "PutNextRectangle_200Rectangles_RectanglesWithNoIntersects")]
    public void PutNextRectangle_ManyRectangles_RectanglesWithNoIntersects(int rectanglesCount)
    {
        for (var i = 0; i < rectanglesCount; i++)
        {
            sut.PutNextRectangle(new Size(10, 10));
        }

        HasIntersectedRectangles(sut.Rectangles).Should().BeFalse();
    }

    private bool HasIntersectedRectangles(IList<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count - 1; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }

        return false;
    }
}