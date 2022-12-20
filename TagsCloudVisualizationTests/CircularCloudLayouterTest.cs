using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTest
{
    private CircularCloudLayouter _layouter;
    private PointF _center;

    private LayoutOptions _options;

    [SetUp]
    public void Setup()
    {
        var spiral = new ArithmeticSpiral();
        _center = new Point(0, 0);
        _layouter = new CircularCloudLayouter(spiral);
        _options = new LayoutOptions(_center, 0.1f);
    }

    [Test]
    public void Constructor_NotPositiveSpiralStep_Throw()
    {
        new Action(() => { new CircularCloudLayouter(new ArithmeticSpiral(-1)); })
            .Should()
            .Throw<ArgumentException>();
    }

    [TestCase(0, 0, TestName = "Zero size")]
    [TestCase(5, 0, TestName = "Zero height")]
    [TestCase(0, 5, TestName = "Zero width")]
    [TestCase(-1, 5, TestName = "Negative width")]
    [TestCase(5, -1, TestName = "Negative height")]
    public void PutNextRectangle_NotPositiveOrSingleSideSize_NotIsSuccess(int width, int height)
    {
        var rectangle = _layouter.PutNextRectangle(new Size(0, 0), _options);
        rectangle.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_ZeroSize_NotIsSuccess()
    {
        var rectangle = _layouter.PutNextRectangle(new Size(0, 0), _options);
        rectangle.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_RightSize_RectangleSizeEqual()
    {
        var random = new Random();
        var width = random.Next(1, 100);
        var height = random.Next(1, 100);

        var rectangleResult = _layouter.PutNextRectangle(new Size(width, height), _options);

        using (new AssertionScope())
        {
            rectangleResult.IsSuccess.Should().BeTrue();
            var rectangle = rectangleResult.GetValueOrThrow();
            rectangle.Width.Should().Be(width);
            rectangle.Height.Should().Be(height);
        }
    }


    [Test]
    public void PutNextRectangle_FirstRectangle_ShiftToCenterOfRectangle()
    {
        const int x = 4;
        const int y = 6;
        const int width = 9;
        const int height = 12;

        _options = new LayoutOptions(new PointF(x, y), 0.1f);
        var rectangleResult = _layouter.PutNextRectangle(new Size(width, height), _options);

        const float expectedX = x - (float)width / 2;
        const float expectedY = y - (float)height / 2;

        using (new AssertionScope())
        {
            rectangleResult.IsSuccess.Should().BeTrue();
            var rectangle = rectangleResult.GetValueOrThrow();
            rectangle.Width.Should().Be(width);
            rectangle.Height.Should().Be(height);
            rectangle.X.Should().Be(expectedX);
            rectangle.Y.Should().Be(expectedY);
        }
    }


    [TestCase(4, 4)]
    [TestCase(7, 3)]
    [TestCase(5, 7)]
    public void PutNextRectangle_TwoRectangles_NotIntersect(int width, int height)
    {
        var firstRectangleResult = _layouter.PutNextRectangle(new Size(width, height), _options);
        var secondRectangleResult = _layouter.PutNextRectangle(new Size(width, height), _options);


        using (new AssertionScope())
        {
            firstRectangleResult.IsSuccess.Should().BeTrue();
            secondRectangleResult.IsSuccess.Should().BeTrue();

            var firstRectangle = firstRectangleResult.GetValueOrThrow();
            var secondRectangle = secondRectangleResult.GetValueOrThrow();

            firstRectangle
                .IntersectsWith(secondRectangle)
                .Should()
                .BeFalse();
        }
    }

    [Test]
    public void PutNextRectangle_ManyRectangles_AllNotIntersect()
    {
        var random = new Random();
        var rectangles = new List<RectangleF>();


        for (int i = 0; i < 100; i++)
        {
            var newX = random.Next(40, 100);
            var newY = random.Next(40, 100);
            var putResult = _layouter.PutNextRectangle(new Size(newX, newY), _options);

            if (putResult.IsSuccess)
                rectangles.Add(putResult.GetValueOrThrow());
        }

        foreach (var rectangle in rectangles)
        {
            rectangles.Where(rect => rect != rectangle)
                .Should().AllSatisfy(x => rectangle.IntersectsWith(x).Should().BeFalse());
        }
    }


    [Test]
    public void PutNextRectangle_ManyRectangles_AllPutInLayout()
    {
        var random = new Random();
        var rectangles = new List<RectangleF>();

        for (int i = 0; i < 100; i++)
        {
            var newX = random.Next(40, 100);
            var newY = random.Next(40, 100);

            var putResult = _layouter.PutNextRectangle(new Size(newX, newY), _options);

            if (putResult.IsSuccess)
                rectangles.Add(putResult.GetValueOrThrow());
        }

        _layouter.Rectangles.Should().BeEquivalentTo(rectangles);
    }


    [TearDown]
    public void TearDown()
    {
    }
}