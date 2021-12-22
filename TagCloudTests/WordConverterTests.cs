using System;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.TextHandlers.Converters;

namespace TagCloudTests
{
    [TestFixture]
    public class WordConverterTests
    {
        [Test]
        public void Convert_ShouldConvertWord()
        {
            const string? text = "TeSt";
            var expected = text.ToLower().AsResult();
            var sut = new ConvertersPool(new IConverter[] { new ToLowerConverter() });
            var converted = sut.Convert(text);

            converted.Should().BeEquivalentTo(expected);
        }
    }
}