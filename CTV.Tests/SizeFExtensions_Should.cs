using System;
using System.Drawing;
using CTV.Common;
using FluentAssertions;
using NUnit.Framework;

namespace CTV.Tests
{
    public class SizeFExtensions_Should
    {
        [TestCase(1.2f, 2.3f, 2, 3, TestName = "When coordinates fractional part less than 0.5")]
        [TestCase(1.5f, 2.5f, 2, 3, TestName = "When coordinates fractional part equals 0.5")]
        [TestCase(1.7f, 2.7f, 2, 3, TestName = "When coordinates fractional part less than 0.5")]
        public void Return_SizeWithCeilingWidthAndHeight(
            float startWidth, float startHeight,
            int expectedWidth, int expectedHeight)
        {
            var expected = new Size(expectedWidth, expectedHeight);
            var initialSizeF = new SizeF(startWidth, startHeight);

            var result = initialSizeF.ToCeilSize();

            result.Should().BeEquivalentTo(expected);
        }
    }
}