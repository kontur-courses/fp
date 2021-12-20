using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Infrastructure.Layouter;
using TagCloud.Infrastructure.Monad;

namespace TagCloudTests.Infrastructure.Layouter;

internal class CloudLayouterFactoryTests
{
    private CloudLayouterFactory sut;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var circularLayouter = new CircularCloudLayouter(new Point(0, 0));
        var mockLayouter = new FakeCloudLayouter();

        sut = new CloudLayouterFactory(new ICloudLayouter[] { circularLayouter, mockLayouter }, circularLayouter);
    }

    [TestCase("circularcloudlayouter", typeof(CircularCloudLayouter))]
    [TestCase("fakecloudlayouter", typeof(FakeCloudLayouter))]
    [TestCase("unknown", typeof(CircularCloudLayouter))]
    public void Create_ShouldReturnCorrectLayouter(string layouterName, Type expectedType)
    {
        var actual = sut.Create(layouterName);

        actual.Should().BeOfType(expectedType);
    }

    public class FakeCloudLayouter : ICloudLayouter
    {
        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            return Result.Ok(new Rectangle());
        }

        public Rectangle[] GetLayout()
        {
            return Array.Empty<Rectangle>();
        }
    }
}