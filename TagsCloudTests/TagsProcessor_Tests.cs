using System;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TagsCloud;
using TagsCloud.Interfaces;

namespace TagsCloudTests
{
	[TestFixture]
	public class TagsProcessor_Tests
	{
		private TagsProcessor tagsProcessor;
		private IWordsProcessor wordsProcessor;
		private FontSettings settings;
		private IImageHolder imageHolder;

		[SetUp]
		public void SetUp()
		{
			wordsProcessor = Substitute.For<IWordsProcessor>();
			settings = new FontSettings();
			imageHolder = Substitute.For<IImageHolder>();
			tagsProcessor = new TagsProcessor(wordsProcessor, settings, imageHolder);
		}

		[Test]
		public void CalculateFontSize_ReturnsSameSizes_WhenMaxAndMinSizesAreEqual()
		{
			const int expectedSize = 15;
			settings.MaxFontSize = expectedSize;
			settings.MinFontSize = expectedSize;
			var randomizer = new Random();
			var words = Enumerable.Range(0, 10)
				.Select(_ => randomizer.Next(5, 30))
				.Select(s => new Word("a", s));

			var actualSizes = words.Select(w => tagsProcessor.CalculateFontSize(w).Value);
			actualSizes.Should().AllBeEquivalentTo(expectedSize);
		}

		[TestCase(100, 10, 50, 50, TestName = "Max size WHEN Calculated size grater than max size")]
		[TestCase(2, 10, 50, 10, TestName = "Min size WHEN Calculated size less than min size")]
		[TestCase(1, 10, 50, 10, TestName = "Min size WHEN Calculated size is zero")]
		[TestCase(50, 10, 50, 45, TestName = "45 WHEN Word frequency = 50")]
		public void CalculateFontSize_Returns(int wordFrequency,
												int minFontSize, 
												int maxFontSize,
												int expectedFontSize)
		{
			settings.MaxFontSize = maxFontSize;
			settings.MinFontSize = minFontSize;
			var word = new Word("a", wordFrequency);

			var actualFontSize = tagsProcessor.CalculateFontSize(word).Value;
			actualFontSize.Should().Be(expectedFontSize);
		}
	}
}