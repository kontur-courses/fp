using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTests;

[TestFixture]
public class TxtFileLoaderTests
{
    [TestCase("")]
    [TestCase(null)]
    [TestCase("file.text")]
    [TestCase(@"A:\PathIsNotExists\ItIsFake.jpeg")]
    [TestCase(@"ItIsFake.jpeg")]
    public void Load_ExceptionOnIncorrectArgument(string value)
    {
        var loader = new TxtFileLoader();
        loader.Load(value).IsSuccess.Should().BeFalse();
    }

    [Test]
    public void Load_ReadCorrect()
    {
        var loader = new TxtFileLoader();
        const string path = "Words.txt";

        var actual = loader.Load(path);

        var expected = File.ReadAllText(path);
        actual.Value.Should().Be(expected);
    }
}