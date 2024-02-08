using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using TagsCloudContainer.Algorithm;
using TagsCloudContainer.Infrastucture;
using TagsCloudContainer.Infrastucture.Settings;

namespace TagsCloudContainerTests
{
    public class RectanglePlaces_Should
    {
        private AlgorithmSettings algSettings;
        private ImageSettings imgSettings;
        private RectanglePlacer placer;

        [SetUp]
        public void SetUp()
        {
            algSettings = new AlgorithmSettings();
            imgSettings = new ImageSettings();
            placer = new RectanglePlacer(algSettings, imgSettings);
        }

        [TestCase(0, 1, TestName = "Return fail when width is zero")]
        [TestCase(1, 0, TestName = "Return fail when height is zero")]
        [TestCase(0, 0, TestName = "Return fail when height and width are zero")]
        [TestCase(-1, 12, TestName = "Return fail when width is a negative number")]
        [TestCase(34, -1, TestName = "Return fail when height is a negative number")]
        [TestCase(-32, -54, TestName = "Return fail when width and height are negative numbers")]
        public void ReturnResultFail_WhenRectangleDimensionsIncorrect(float width, float height)
        {
            var size = new SizeF(width, height);
            var cloudRectangles = new List<TextRectangle>();
            
            var rect = placer.GetPossibleNextRectangle(cloudRectangles, size);

            rect.IsSuccess.Should().BeFalse();
            rect.Error.Should().Be("The width and height of the rectangle must be positive numbers");
        }

        [TestCase(0, 1, TestName = "Return fail when deltaRadius is zero")]
        [TestCase(1, 0, TestName = "Return fail when deltaAngle is zero")]
        [TestCase(0, 0, TestName = "Return fail when deltaRadius and deltaAngle are zero")]
        public void ReturnResultFail_WhenAlgorithmSettingsIncorrect(float deltaRadius, float deltaAngle)
        {
            algSettings.DeltaRadius = deltaRadius;
            algSettings.DeltaAngle = deltaAngle;
            var size = new SizeF(1, 1);
            var cloudRectangles = new List<TextRectangle>();

            var rect = placer.GetPossibleNextRectangle(cloudRectangles, size);

            rect.IsSuccess.Should().BeFalse();
            rect.Error.Should().Be("DeltaRadius and DeltaAngle cannot be equal to zero");
        }
    }
}
