using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ResultOf;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.WordsProcessing;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class WordsCloudBuilder_Should
    {
        private IWordsProvider wordsProvider;
        private IWeighter weighter;
        private ISizeConverter sizeConverter;
        private ICloudLayouter cloudLayouter;
        private WordsCloudBuilder wordsCloudBuilder;
        private Size defaultSize = new Size(200, 100);
        private Rectangle rectangle;
        private Font defaultFont = new Font("Times New Roman", 100);

        [SetUp]
        public void SetUp()
        {
            wordsProvider = Substitute.For<IWordsProvider>();
            weighter = Substitute.For<IWeighter>();
            sizeConverter = Substitute.For<ISizeConverter>();
            cloudLayouter = Substitute.For<ICloudLayouter>();
            wordsCloudBuilder = new WordsCloudBuilder(wordsProvider, cloudLayouter, sizeConverter, weighter);
            rectangle = new Rectangle(new Point(0, 0), defaultSize);
            cloudLayouter.PutNextRectangle(defaultSize).Returns(Result.Ok(rectangle));
            var words = new[] {"a", "a", "b", "b", "b"};
            wordsProvider.Provide().Returns(Result.Ok(words.AsEnumerable()));
            var weighted = new[] {new WeightedWord("a", 2), new WeightedWord("b", 3)};
            weighter.WeightWords(words).Returns(Result.Ok(weighted.AsEnumerable()));
            sizeConverter.Convert(weighted).Returns(Result.Ok(new[]{new SizedWord("a", defaultFont, defaultSize), new SizedWord("b", defaultFont, defaultSize)}.AsEnumerable()));
        }

        [Test]
        public void BuildCloud_And_ReturnWords()
        {
            var expected = new []{new Word("a", defaultFont, rectangle), new Word("b", defaultFont, rectangle)}; 
            wordsCloudBuilder.Build().GetValueOrThrow().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void BuildCloud_ReturnEmptySequence_OnEmptyInput()
        {
            wordsProvider.Provide().Returns(Result.Ok(Enumerable.Empty<string>()));
            weighter.WeightWords(null).ReturnsForAnyArgs(Result.Ok(Enumerable.Empty<WeightedWord>()));
            sizeConverter.Convert(null).ReturnsForAnyArgs(Result.Ok(Enumerable.Empty<SizedWord>()));
            wordsCloudBuilder.Build().GetValueOrThrow().Should().BeEmpty();
        }

        [Test]
        public void BuildCloud_WhenProviderFail_Fail()
        {
            wordsProvider.Provide().Returns(Result.Fail<IEnumerable<string>>("provider fail"));
            wordsCloudBuilder.Build().Error.Should().BeEquivalentTo("provider fail");
        }

        [Test]
        public void BuildCloud_WhenWeighterFail_Fail()
        {
            weighter.WeightWords(Arg.Any<IEnumerable<string>>()).Returns(Result.Fail<IEnumerable<WeightedWord>>("weighter fail"));
            wordsCloudBuilder.Build().Error.Should().BeEquivalentTo("weighter fail");
        }

        [Test]
        public void BuildCloud_WhenSizerFail_Fail()
        {
            sizeConverter.Convert(Arg.Any<IEnumerable<WeightedWord>>()).Returns(Result.Fail<IEnumerable<SizedWord>>("sizer fail"));
            wordsCloudBuilder.Build().Error.Should().BeEquivalentTo("sizer fail");
        }

        [Test]
        public void BuildCloud_WhenLayouterFail_Fail()
        {
            cloudLayouter.PutNextRectangle(Arg.Any<Size>()).Returns(Result.Fail<Rectangle>("layouter fail"));
            wordsCloudBuilder.Build().Error.Should().BeEquivalentTo("layouter fail");
        }

        [Test]
        public void BuildCloud_WhenSeveralFail_FirstOnly()
        {
            sizeConverter.Convert(Arg.Any<IEnumerable<WeightedWord>>()).Returns(Result.Fail<IEnumerable<SizedWord>>("sizer fail"));
            cloudLayouter.PutNextRectangle(Arg.Any<Size>()).Returns(Result.Fail<Rectangle>("layouter fail"));
            wordsCloudBuilder.Build().Error.Should().BeEquivalentTo("sizer fail");
        }
    }
}