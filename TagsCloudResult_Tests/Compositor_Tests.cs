using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudResult;
using TagsCloudResult.Infrastructure.Common;
using TagsCloudResult.Layouter;

namespace TagsCloudResult_Tests
{
    [TestFixture]
    public class CompositorTests
    {
        private static IEnumerable<LayoutWord> words =
            new[]
            {
                new LayoutWord("First", new SolidBrush(Color.Red), new Font("Arial", 7), new Size(4, 6)),
                new LayoutWord("Second", Brushes.Gray, new Font("Arial", 7), new Size(12, 12)),
            };

        private static AppSettings settings = AppSettingsForTests.Settings;
       
        [Test]
        public void ShouldReturnAllWords_IfGetRightInput()
        {
            var cloudLayouter = new CircularCloudLayouter();
            var actual = Compositor.Composite(words, cloudLayouter.PutNextRectangle, settings);

            actual.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ShouldFail_IfGetIncorrectWordSize()
        {
            var badWords = new[]
            {
                new LayoutWord("First", new SolidBrush(Color.Red), new Font("Arial", 7), new Size(-4, 6))
            };
            
            var cloudLayouter = new CircularCloudLayouter();
            var actual = Compositor.Composite(badWords, cloudLayouter.PutNextRectangle, settings);

            actual.IsSuccess.Should().BeFalse();
        }
    }
}