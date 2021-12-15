using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Infrastructure.FileReader;

namespace TagCloudTests.Infrastructure.FileReader;

internal class PlainTextFileReaderTests
{
    private const string FileName = $"{nameof(PlainTextFileReader)}Test.txt";
    private readonly PlainTextFileReader sut = new();

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        using var fs = File.Open(FileName, FileMode.Create);
        using var sw = new StreamWriter(fs);
        sw.Write($"firstline{Environment.NewLine}secondline{Environment.NewLine}thirdline{Environment.NewLine}{Environment.NewLine}");
    }

    [Test]
    public void GetLines_ShouldReturnCorrectLinesEnumerable()
    {
        var expected = new[] { "firstline", "secondline", "thirdline", "" };

        var actual = sut.GetLines(FileName);

        actual.GetValueOrThrow().Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetLines_ShouldReturnFail_WhenFileDoesNotExists()
    {
        const string path = "notExistingFile.txt";

        var actual = sut.GetLines(path);

        actual.IsSuccess.Should().BeFalse();
        actual.Error.Should().Be($"The file does not exist {path}");
    }
}