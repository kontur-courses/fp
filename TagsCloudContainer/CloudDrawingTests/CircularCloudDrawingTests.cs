using System.Drawing;
using CloudDrawing;
using CloudLayouter;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace CloudDrawingTests
{
    public class CircularCloudDrawingTests
    {
        private CircularCloudDrawing circularCloudDrawing;

        [SetUp]
        public void SetUp()
        {
            var cloudLayouter =  A.Fake<ICloudLayouter>();
            A.CallTo(() => cloudLayouter.PutNextRectangle(Size.Empty))
                .WithAnyArguments()
                .Returns(new Rectangle(0, 0, 1000, 1000));
            A.CallTo(() => cloudLayouter.PutNextRectangle(Size.Empty))
                .WithAnyArguments()
                .Returns(new Rectangle(5, 5, 20, 30))
                .Once();
            circularCloudDrawing = new CircularCloudDrawing(cloudLayouter);
            circularCloudDrawing.SetOptions(ImageSettings.GetImageSettings("Cyan", 100,100).GetValueOrThrow());
        }

        [Test]
        public void DrawWordsSuccessfully()
        {
            circularCloudDrawing.DrawWords(new[] {("Hi", 5)},
                    WordDrawSettings.GetWordDrawSettings("Arial", "Cyan", false).GetValueOrThrow())
                .IsSuccess.Should().BeTrue();
        }
        
        [Test]
        public void DrawWordsError_WhenWordIsNotLocatedInArea()
        {
            circularCloudDrawing.DrawWords(new[] {("Hi", 5), ("Zakhar", 100)},
                    WordDrawSettings.GetWordDrawSettings("Arial", "Cyan", false).GetValueOrThrow())
                .IsSuccess.Should().BeFalse();
        }
    }
}