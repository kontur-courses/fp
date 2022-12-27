using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloudPainter.Common;
using TagCloudPainter.ResultOf;
using TagCloudPainter.Sizers;

namespace TagCloudPainter.Tests;

public class WordSizerTests
{
    public WordSizer Sizer { get; set; }
    public ImageSettings Settings { get; set; }

    [SetUp]
    public void Setup()
    {
        Settings = new ImageSettings
        {
            BackgroundColor = Color.Bisque,
            Font = new Font("Arial", 10),
            Size = new Size(500, 500)
        };
        var provider = new ImageSettingsProvider { ImageSettings = Settings };
        Sizer = new WordSizer(provider);
    }

    [TestCase("", TestName = "{m}_Empty")]
    [TestCase(" ", TestName = "{m}_WhiteSpace")]
    [TestCase(null, TestName = "{m}_null")]
    public void GetTagSize_Should_Fail_On_(string word)
    {
        var result = Sizer.GetTagSize(word, 1);

        result.Should().BeEquivalentTo(Result.Fail<Size>("word is null or white space"));
    }

    [TestCase(0, TestName = "{m}_ZeroCount")]
    [TestCase(-1, TestName = "{m}_NegativeCount")]
    public void GetTagSize_Should_Fail_On_(int count)
    {
        var result = Sizer.GetTagSize("word", count);

        result.Should().BeEquivalentTo(Result.Fail<Size>("the word does not appear in the text"));
    }

    [TestCase("word", 1)]
    [TestCase("1", 3)]
    [TestCase("word", 4)]
    public void GetSize_Basic_Functionality_Test(string word, int count)
    {
        var width = word.Length * (int)(Settings.Font.Size + 1) + 3 * (count - 1);
        var height = (Settings.Size.Height + Settings.Size.Width) / 40 + 2 * (count - 1);

        var size = Sizer.GetTagSize(word, count).GetValueOrThrow();

        size.Should().BeEquivalentTo(new Size(width, height));
    }

    [Test]
    public void GetSize_Fail_On_null_ImageSettings()
    {
        var provider = new ImageSettingsProvider { ImageSettings = null };
        var sizer = new WordSizer(provider);

        var result = sizer.GetTagSize("word", 1);

        result.Should().BeEquivalentTo(Result.Fail<Size>("settings could not be read"));
    }
}