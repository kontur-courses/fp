using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using ResultMonad;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.PointGenerators;


namespace TagsCloudTests
{
    public class LayouterTests
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void Setup()
        {
            var pointGenerator = new ArchimedeanSpiralPointGenerator(new Point(0, 0));
            layouter = new CircularCloudLayouter(pointGenerator);
        }
        
        [Test]
        public void PutNextRectangle_ShouldBeResultOK_WhenValidSize()
        {
            var actual = layouter.PutNextRectangle(new Size(4, 5));
            
            actual.IsSuccess.Should().BeTrue();
        }
        

        [Test]
        public void PutNextRectangle_ShouldBeResultFail_WhenNegativeSize()
        {
            var actual = layouter.PutNextRectangle(new Size(-1, -1));

            var expected = Result.Fail<Rectangle>("Rectangle width should be > 0");
            actual
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}