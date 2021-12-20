﻿using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudApp.Providers;
using TagsCloudContainer.TagsCloudLayouter;

namespace TagsCloudContainerTests
{
    [TestFixture]
    internal class OvalCloudLayouterTests
    {
        private OvalCloudLayouter layouter;
        private const int X = 500;
        private const int Y = 500;
        private const int Count = 500;

        [SetUp]
        public void SetUp()
        {
            var settings = SettingsProvider.GetSettings();
            var cloudSettings = CloudSettingsProvider.GetSettings();
            settings.ImageSize = new Size(X * 2, Y * 2);
            layouter = new OvalCloudLayouter(settings, cloudSettings);
        }

        [Test]
        public void Should_HaveCenter_AfterCreation()
        {
            layouter.Cloud.Center
                .Should().Be(new Point(X, Y));
        }

        [Test]
        public void Should_HaveNoRectangles_AfterCreation()
        {
            layouter.Cloud.Rectangles.Should().BeEmpty();
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        [TestCase(0, -0)]
        [TestCase(10, 0)]
        [TestCase(-10, 10)]
        public void Should_Throw_When_TryingToPutIncorrectSize(int width, int height)
        {
            layouter.PutNextRectangle(new Size(width, height)).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void Should_ReturnRectangleWithSameSize_AfterPutNextRectangle()
        {
            var sizeToAdd = new Size(X, Y);
            layouter.PutNextRectangle(sizeToAdd).Value.Size
                .Should().Be(sizeToAdd);
        }

        [TestCase(1111, 30, 50)]
        [TestCase(323, 5, 60)]
        [TestCase(232, 50, 120)]
        public void Should_ContainSameNumberOfRectanglesAsWerePut(int seed,
            int minSize, int maxSize)
        {
            layouter.PutManyRectangles(Count, new Random(seed), minSize, maxSize);
            layouter.Cloud.Rectangles.Count.Should().Be(Count);
        }

        [Test]
        public void Should_ContainAllAddedRectangles()
        {
            var sizes = new List<Size>();

            for (var i = 1; i <= 5; i++)
            {
                var size = new Size(X, Y);
                sizes.Add(size);
                layouter.PutNextRectangle(size);
            }

            layouter.Cloud.Rectangles
                .Select(rect => rect.Size)
                .Should()
                .BeEquivalentTo(sizes);
        }

        [TestCase(666, 240, 400)]
        [TestCase(223, 5, 35)]
        public void Should_NotContain_IntersectedRectangles(int seed,
            int minSize, int maxSize)
        {
            layouter.PutManyRectangles(Count, new Random(seed), minSize, maxSize);

            layouter.Cloud.Rectangles
                .All(x => layouter.Cloud.Rectangles.Count(y => y.IntersectsWith(x)) == 1)
                .Should()
                .BeTrue();
        }

        [TestCase(321, 55, 60)]
        [TestCase(123, 80, 150)]
        public void Should_PutManyRectangles_Fast(int seed,
            int minSize, int maxSize)
        {
            Action action = ()
                => layouter.PutManyRectangles(Count, new Random(seed), minSize, maxSize);
            action.ExecutionTime().Should().BeLessThan(1.Seconds());
        }

        [TestCase(321, 35, 75)]
        [TestCase(123, 350, 800)]
        public void Should_CreateTightLayout(int seed,
            int minSize, int maxSize)
        {
            layouter.PutManyRectangles(Count, new Random(seed), minSize, maxSize);
            var radius = layouter.Cloud.CalculateCloudRadius();
            var circleArea = Math.PI * radius * radius;
            var layoutArea = layouter.Cloud.Rectangles
                .Select(rectangle => rectangle.Height * rectangle.Width)
                .Sum();

            circleArea.Should().BeInRange(layoutArea, 1.25 * layoutArea);
        }
    }
}
