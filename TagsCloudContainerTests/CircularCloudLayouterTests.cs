using FluentAssertions;
using System.Drawing;
using TagsCloudContainer.TagsCloud;
using TagsCloudContainer.Utility;

namespace TagsCloudContainerTests
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter sut;
        private Point center = new(720, 720);

        [SetUp]
        public void SetUp()
        {
            var spiral = new Spiral(center, 0.02, 0.01);
            sut = new CircularCloudLayouter(center, spiral);
        }

        [Test]
        public void Constructor_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(center, new Spiral(center, 0.02, 0.01)));
        }

        [TestCase(-4, 16, TestName = "with negative rectangle width")]
        [TestCase(77, -8, TestName = "with negative rectangle height")]
        public void PutNextRectangle_InvalidSize_ReturnsFailureResult(int rectangleWidth, int rectangleHeight)
        {
            var result = sut.PutNextRectangle(new Size(rectangleWidth, rectangleHeight));

            Assert.IsFalse(result.IsSuccess);
            Assert.IsInstanceOf<Result<Rectangle>>(result);
        }

        [Test]
        public void PutNextRectangle_ShouldReturnCorrectRectangleSize()
        {
            var expectedSize = new Size(8, 8);
            var rectangleResult = sut.PutNextRectangle(expectedSize);

            rectangleResult.OnSuccess(rectangle =>
            {
                rectangle.Size.Should().Be(expectedSize);
            });

            rectangleResult.OnFail(error =>
            {
                Assert.Fail($"Test failed with error: {error}");
            });
        }

        [TestCase(10, TestName = "10 rectangles with maximum density")]
        [TestCase(100, TestName = "100 rectangles with maximum density")]
        [TestCase(200, TestName = "200 rectangles with maximum density")]
        public void PutNextRectangle_RectanglesShouldHaveMaximumDensity(int rectanglesCount)
        {
            var center = new Point(0, 0);
            var spiral = new Spiral(center, 0.02, 0.01);
            var layouter = new CircularCloudLayouter(center, spiral);
            var rectangleSize = new Size(10, 10);

            GenerateRectangles(layouter, rectangleSize, rectanglesCount);

            CheckDensity(layouter.Rectangles).Should().BeTrue();
        }

        private void GenerateRectangles(CircularCloudLayouter layouter, Size rectangleSize, int rectanglesCount)
        {
            for (int i = 0; i < rectanglesCount; i++)
            {
                layouter.PutNextRectangle(rectangleSize);
            }
        }

        private bool CheckDensity(IList<Rectangle> rectangles)
        {
            double maxDistance = 0;

            foreach (var rect in rectangles)
            {
                var distance = Math.Sqrt(Math.Pow(rect.X - rect.Width / 2, 2) + Math.Pow(rect.Y - rect.Height / 2, 2));
                maxDistance = Math.Max(maxDistance, distance);
            }

            var circleArea = Math.PI * Math.Pow(maxDistance, 2);
            var rectanglesArea = rectangles.Sum(rect => rect.Width * rect.Height);
            var density = rectanglesArea / circleArea;
            var thresholdDensity = 1;

            return density <= thresholdDensity;
        }

        [Test]
        public void PutNextRectangle_FirstRectangle_ShouldHaveCenterAtLayoutCenter()
        {
            var rectangleResult = sut.PutNextRectangle(new Size(8, 8));

            rectangleResult.OnSuccess(rectangle =>
            {
                var expectedRectangleCenter =
                    new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
                expectedRectangleCenter.Should().Be(center);
            });

            rectangleResult.OnFail(error =>
            {
                Assert.Fail($"Test failed with error: {error}");
            });
        }

        [Test]
        public void PutNextRectangle_TwoRectangles_SecondRectangleCenterShouldNotBeAtLayoutCenter()
        {
            sut.PutNextRectangle(new Size(8, 8));

            var secondRectangleResult = sut.PutNextRectangle(new Size(6, 6));

            secondRectangleResult.OnSuccess(secondRectangle =>
            {
                var expectedRectangleCenter = new Point(
                    secondRectangle.Left + secondRectangle.Width / 2,
                    secondRectangle.Top + secondRectangle.Height / 2);

                expectedRectangleCenter.Should().NotBe(center);
            });

            secondRectangleResult.OnFail(error =>
            {
                Assert.Fail($"Test failed with error: {error}");
            });
        }

        [Test]
        public void PutNextRectangle_TwoRectangles_ShouldNotIntersect()
        {
            var firstRectangleResult = sut.PutNextRectangle(new Size(8, 8));

            firstRectangleResult.OnSuccess(firstRectangle =>
            {
                var secondRectangleResult = sut.PutNextRectangle(new Size(77, 77));

                secondRectangleResult.OnSuccess(secondRectangle =>
                {
                    secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
                });

                secondRectangleResult.OnFail(error =>
                {
                    Assert.Fail($"Test failed with error: {error}");
                });
            });

            firstRectangleResult.OnFail(error =>
            {
                Assert.Fail($"Test failed with error: {error}");
            });
        }

        [TestCase(10, TestName = "10 rectangles WithNoIntersects")]
        [TestCase(100, TestName = "100 rectangles WithNoIntersects")]
        [TestCase(200, TestName = "200 rectangles WithNoIntersects")]
        public void PutNextRectangle_GeneratesRectanglesWithoutIntersects(int rectanglesCount)
        {
            GenerateRectangles(rectanglesCount, new Size(10, 10));

            Assert.IsFalse(HasIntersectedRectangles(sut.Rectangles));
        }

        private bool HasIntersectedRectangles(IList<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void GenerateRectangles(int count, Size rectangleSize)
        {
            for (var i = 0; i < count; i++)
            {
                sut.PutNextRectangle(rectangleSize);
            }
        }
    }
}
