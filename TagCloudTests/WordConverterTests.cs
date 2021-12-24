using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.TextHandlers.Converters;

namespace TagCloudTests;

[TestFixture]
public class WordConverterTests
{
    [Test]
    public void Convert_ShouldConvertWord()
    {
        const string? text = "TeSt";
        var expected = text.ToLower();
        var sut = new ConvertersPool(new IConverter[] { new ToLowerConverter() });

        var converted = sut.Convert(new[] { text }).Value.ToArray();

        converted.Should().BeEquivalentTo(new[] { expected });
    }
}