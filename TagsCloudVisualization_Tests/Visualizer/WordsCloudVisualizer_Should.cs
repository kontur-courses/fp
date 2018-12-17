using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ResultOf;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization_Tests.Visualizer
{
    [TestFixture]
    public class WordsCloudVisualizer_Should
    {
        private WordsCloudVisualizer visualizer;
        private IWordsCloudBuilder cloudBuilder;
        private Palette palette;
        private Size pictureSize;
        [SetUp]
        public void SetUp()
        {
            cloudBuilder = Substitute.For<IWordsCloudBuilder>();
            cloudBuilder.Build().Returns(Result.Ok(Enumerable.Empty<Word>()));
            palette = new Palette("DimGray", "AliceBlue");
            pictureSize = new Size(800, 800);
            visualizer = new WordsCloudVisualizer(cloudBuilder, palette, pictureSize);
        }

        [Test]
        public void Draw_WithGivenSize()
        {
            visualizer.Draw().GetValueOrThrow().Size.Should().Be(pictureSize);
        }

        [Test]
        public void Draw_ColorSettingsFail_Fail()
        {
            visualizer = new WordsCloudVisualizer(cloudBuilder, new Palette("gg", "AliceBlue"), pictureSize);
            visualizer.Draw().Error.Should().StartWith("No such color name");
        }

        [Test]
        public void Draw_BuilderFail_Fail()
        {
            cloudBuilder.Build().Returns(Result.Fail<IEnumerable<Word>>("builder fail"));
            visualizer.Draw().Error.Should().Be("builder fail");
        }

        [Test]
        public void Draw_BitmapCreationFail_Fail()
        {
            cloudBuilder.Radius.Returns(5);
            visualizer = new WordsCloudVisualizer(cloudBuilder, palette, new Size(1, 1));
            visualizer.Draw().Error.Should().StartWith("Cloud can't fit in given size");
        }

        [Test]
        public void Draw_SeveralFails_FirstOnly()
        {
            cloudBuilder.Build().Returns(Result.Fail<IEnumerable<Word>>("builder fail"));
            visualizer = new WordsCloudVisualizer(cloudBuilder, new Palette("gg", "AliceBlue"), pictureSize);
            visualizer.Draw().Error.Should().StartWith("No such color name");
        }
    }
}
