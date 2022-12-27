using FluentAssertions;
using System.Drawing;
using NUnit.Framework.Interfaces;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Infrastructure.WordLayoutBuilders;

namespace TagsCloudContainer.Tests.LayoutBuilderTests
{
    public class CircularWordLayoutBuilderShould
    {
        private IWordLayoutBuilder layoutBuilder;

        [SetUp]
        public void SetUp()
        {
            layoutBuilder = new CircularWordLayoutBuilder();
        }

        [Test]
        public void AddWord_Should_ReturnFailedResult_WhenSizeIsZero()
        {
            var result = layoutBuilder.AddWord("123", default);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void AddWord_Should_ReturnFailedResult_WhenWordWasAlreadyAdded()
        {
            var word = "123";
            var size = new Size(100, 100);    

            layoutBuilder.AddWord(word, size);
            var result = layoutBuilder.AddWord(word, size);

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void PutNextRectanlge_Should_ReturnNonZeroRectangleInZeroPoint_WhenSizeIsNotZeroAndCenterIsZero()
        {
            var size = new Size(100, 100);
            var word = "123";
            var expectedRectangle = new WordRectangle() { Rectangle = new Rectangle(new Point(-50, -50), size), Word = word };

            layoutBuilder.AddWord(word, size);
            var result = layoutBuilder.Build(default);
            var actualRectangles = result.Value;

            result.IsSuccess.Should().BeTrue();
            actualRectangles.Should().HaveCount(1);
            actualRectangles.Last().Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_Should_AddSecondRectangleOnTheFirstTop_WhenWidthOfFirstIsGreaterThanTheHeight()
        {
            var firstRectangleSize = new Size(200, 100);
            var secondRectangleSize = new Size(100, 100);
            var word = "123";
            var expectedRectangle = new WordRectangle() { Rectangle = new Rectangle(new Point(-50, -150), secondRectangleSize), Word = word };

            layoutBuilder.AddWord("567", firstRectangleSize);
            layoutBuilder.AddWord(word, secondRectangleSize);
            var result = layoutBuilder.Build(default);
            var actualRectangles = result.Value;

            result.IsSuccess.Should().BeTrue();
            actualRectangles.Should().HaveCount(2);
            actualRectangles.Last().Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_Should_AddSecondRectangleOnTheFirstLeft_WhenHeightOfFirstIsGreaterThanTheWidth()
        {
            var firstRectangleSize = new Size(100, 200);
            var secondRectangleSize = new Size(100, 100);
            var word = "123";
            var expectedRectangle = new WordRectangle() { Rectangle = new Rectangle(new Point(-150, -50), secondRectangleSize), Word = word };

            layoutBuilder.AddWord("567", firstRectangleSize);
            layoutBuilder.AddWord(word, secondRectangleSize);
            var result = layoutBuilder.Build(default);
            var actualRectangles = result.Value;

            result.IsSuccess.Should().BeTrue();
            actualRectangles.Should().HaveCount(2);
            actualRectangles[1].Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_Should_AddThirdRectangleOnTheFirstBottom_WhenTheSecondIsOnTheFirstTop()
        {
            var firstRectangleSize = new Size(200, 100);
            var rectangleSize = new Size(100, 100);
            var word = "123";
            var expectedRectangle = new WordRectangle() { Rectangle = new Rectangle(new Point(-50, 50), rectangleSize), Word = word };

            layoutBuilder.AddWord("567", firstRectangleSize);
            layoutBuilder.AddWord("222", rectangleSize);
            layoutBuilder.AddWord(word, rectangleSize);
            var result = layoutBuilder.Build(default);
            var actualRectangles = result.Value;

            result.IsSuccess.Should().BeTrue();
            actualRectangles.Should().HaveCount(3);
            actualRectangles.Last().Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_Should_AddRectangleInHole_WhenThereIsASuitableHole()
        {
            var brickSize = new Size(200, 200);
            var smallRectangleSize = new Size(40, 10);
            var word = "123";
            var expectedRectangle1 = new WordRectangle() { Rectangle = new Rectangle(new Point(30, 100), smallRectangleSize), Word = word };
            var expectedRectangle2 = new WordRectangle() { Rectangle = new Rectangle(new Point(-60, 100), smallRectangleSize), Word = word };

            layoutBuilder.AddWord("1", brickSize);
            layoutBuilder.AddWord("2", brickSize);
            layoutBuilder.AddWord("3", brickSize);
            layoutBuilder.AddWord("4", brickSize);
            layoutBuilder.AddWord("5", new SizeF(60, 20));
            layoutBuilder.AddWord("6", new SizeF(180, 100));
            layoutBuilder.AddWord("7", brickSize);
            layoutBuilder.AddWord("8", brickSize);
            layoutBuilder.AddWord(word, smallRectangleSize);

            var result = layoutBuilder.Build(default);
            var actualRectangles = result.Value;

            result.IsSuccess.Should().BeTrue();
            actualRectangles.Should().HaveCount(9);
            actualRectangles.Last().Should().Match<WordRectangle>(wr => wr.Word == word && (wr.Rectangle == expectedRectangle1.Rectangle || 
                                                                                            wr.Rectangle == expectedRectangle2.Rectangle));
        }

        [Test]
        public void PutNextRectangle_Should_PutBigRectanglesAroundSmallOneInCenter()
        {
            var smallRectangleSize = new Size(100, 100);
            var bigHorizontalRectangleSize = new Size(200, 100);
            var bigVerticalRectangleSize = new Size(100, 200);
            var expectedRectangleSize = new Size(50, 100);
            var word = "123";
            var expectedRectangle = new WordRectangle() { Rectangle = new Rectangle(new Point(50, -50), expectedRectangleSize), Word = word };

            layoutBuilder.AddWord("1", smallRectangleSize);
            layoutBuilder.AddWord("2", bigHorizontalRectangleSize);
            layoutBuilder.AddWord("3", bigVerticalRectangleSize);
            layoutBuilder.AddWord("4", bigHorizontalRectangleSize);
            layoutBuilder.AddWord(word, expectedRectangleSize);
            var result = layoutBuilder.Build(default);
            var actualRectangles = result.Value;

            result.IsSuccess.Should().BeTrue();
            actualRectangles.Should().HaveCount(5);
            actualRectangles.Last().Should().BeEquivalentTo(expectedRectangle);
        }
    }
}