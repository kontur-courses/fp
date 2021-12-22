using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Common.ErrorHandling;
using TagsCloudVisualization.Common.Layouters;
using TagsCloudVisualization.Common.Settings;
using TagsCloudVisualization.Common.TagCloudPainters;
using TagsCloudVisualization.Common.Tags;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class TagCloudPainterTests
    {
        [Test]
        public void Paint_ShouldThrowException_WhenLayouterOverlaped()
        {
            var settings = new CanvasSettings(10,10, Color.Chocolate);
            var layouter = new CircularCloudLayouter(settings);
            var painter = new TagCloudPainter(layouter, settings);
            var defaultTagStyle = new TagStyle(Color.Chocolate, new Font("Arial", 50));
            var tags = new[] { new Tag("test1", defaultTagStyle) };

            var actual = painter.Paint(tags)
                .OnFail(Console.WriteLine);
            
            actual.IsSuccess.Should().BeFalse();
            actual.Should().BeEquivalentTo(Result.Fail<Bitmap>(string.Empty),
                options => options
                    .Excluding(ctx => ctx.Error)
                    .ComparingByMembers(typeof(Result<Bitmap>)));
        }
    }
}