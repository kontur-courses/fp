using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Validators;

namespace TagsCloudTests.ValidatorsShould
{
    [TestFixture]
    public class BitmapValidatorShould
    {
        private BitmapValidator validator = new BitmapValidator();

        [Test]
        public void NewRectangleBiggerThanCanvas_ShouldFail()
        {
            var rect = new Rectangle(0, 0, 100, 100);
            var bitmap = new Bitmap(50, 50);

            ErrorValidate(rect, bitmap, "Rectangle out of bitmap bounds");
        }

        [Test]
        public void NewRectanglePartiallyOnCanvas_ShouldFail()
        {
            var rect = new Rectangle(45, 45, 10, 10);
            var bitmap = new Bitmap(50, 50);

            ErrorValidate(rect, bitmap, "Rectangle out of bitmap bounds");
        }

        [Test]
        public void RectangleWithNegativeParams_ShouldFail()
        {
            var rect = new Rectangle(512, 360, -100, 100);
            var bitmap = new Bitmap(1024, 720);

            ErrorValidate(rect, bitmap, "Rectangle has negative params or zero edge");
        }

        [Test]
        public void RectangleCanBePlacedOnCanvas_ShouldReturnRightResult()
        {
            var rect = new Rectangle(512, 360, 100, 100);
            var bitmap = new Bitmap(1024, 720);

            var res = validator.ValidateNewRectangle(rect, bitmap);

            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be(rect);
        }

        private void ErrorValidate(Rectangle rect, Bitmap canvas, string errorMessage)
        {
            var res = validator.ValidateNewRectangle(rect, canvas);

            res.Invoking(x => x.Value)
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(errorMessage);
        }
    }
}