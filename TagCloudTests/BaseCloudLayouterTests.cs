using System.Collections.Concurrent;
using System.Drawing;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.Abstractions;

namespace TagCloudTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class BaseCloudLayouterTests
{
    [SetUp]
    public void SetUp()
    {
        var layouter = new BaseCloudLayouter(new Point(0, 0), fakePointGenerator);
        layouterByTestId[TestContext.CurrentContext.Test.ID] = layouter;
    }

    private readonly IPointGenerator fakePointGenerator = CreateFakePointGenerator();

    private static IPointGenerator CreateFakePointGenerator()
    {
        var fakeGenerator = A.Fake<IPointGenerator>();
        A.CallTo(() => fakeGenerator.Generate(A<Point>.Ignored))
            .Returns(Enumerable.Range(0, int.MaxValue).Select(n => new Point(n, n)));
        return fakeGenerator;
    }

    private readonly ConcurrentDictionary<string, BaseCloudLayouter> layouterByTestId = new();

    [TestCase(0, 0, TestName = "{m}IsEmpty")]
    [TestCase(0, 10, TestName = "{m}WithZeroWidth")]
    [TestCase(10, 0, TestName = "{m}WithZeroHeight")]
    [TestCase(-10, 10, TestName = "{m}WithNegativeWidth")]
    [TestCase(10, -10, TestName = "{m}WithNegativeHeight")]
    public void PutNextRectangle_ReturnCorrectFail_OnSize(int width, int height)
    {
        var layouter = layouterByTestId[TestContext.CurrentContext.Test.ID];
        var size = new Size(width, height);

        var fail = layouter.PutNextRectangle(size);

        fail.IsFailed.Should().BeTrue();
        fail.Errors.Should().ContainSingle()
            .Subject.Message.Should().Be($"Width and height of the rectangle must be positive, but {size}.");
    }

    [Test]
    public void PutNextRectangle_CenterRectangle_HasCorrectParameters()
    {
        var layouter = layouterByTestId[TestContext.CurrentContext.Test.ID];
        var cloudCenter = layouter.Center;
        var size = new Size(10, 10);
        var expectedPosition = new Point(cloudCenter.X - size.Width / 2, cloudCenter.Y - size.Height / 2);
        var expectedRectangle = new Rectangle(expectedPosition, size);

        var result = layouter.PutNextRectangle(size);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedRectangle);
    }

    [TestCase(2)]
    [TestCase(1000)]
    public void PutNextRectangle_ReturnRectangles_DontIntersect(int rectanglesCount)
    {
        var layouter = layouterByTestId[TestContext.CurrentContext.Test.ID];
        var random = new Random();
        var sizes = Enumerable.Range(1, rectanglesCount)
            .Select(_ => new Size(random.Next(10, 31), random.Next(10, 31)));

        foreach (var size in sizes)
            layouter.PutNextRectangle(size);

        foreach (var rect1 in layouter.Rectangles)
        foreach (var rect2 in layouter.Rectangles.Where(r => r != rect1))
            rect1.IntersectsWith(rect2).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_ThrowInvalidOperationException_OnFinishedPointGenerator()
    {
        var center = new Point(0, 0);
        var finiteGenerator = A.Fake<IPointGenerator>();
        A.CallTo(() => finiteGenerator.Generate(A<Point>.Ignored)).Returns(new[] { center });
        var layouter = new BaseCloudLayouter(center, finiteGenerator);
        var size = new Size(10, 10);
        layouter.PutNextRectangle(size);

        var fail = layouter.PutNextRectangle(size);
        
        fail.IsFailed.Should().BeTrue();
        fail.Errors.Should().ContainSingle()
            .Subject.Message.Should().Be("You are trying to put a new rectangle, but the points sequence has ended.");
    }
}