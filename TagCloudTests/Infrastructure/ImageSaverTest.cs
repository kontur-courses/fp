using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Infrastructure.Saver;

namespace TagCloudTests.Infrastructure;

internal class ImageSaverTest
{
    private const string Filename = $"{nameof(ImageSaver)}Test.";
    private readonly IImageSaver sut = new ImageSaver();

    public static IEnumerable<string> ImageFormats => ImageSaver.ImageFormats.Keys;

    [SetUp]
    public void SetUp()
    {
        CleanUp();
    }

    [TearDown]
    public void TearDown()
    {
        CleanUp();
    }

    [TestCaseSource(typeof(ImageSaverTest), nameof(ImageFormats))]
    public void Save_ShouldCreateFileWithCorrectFormat(string outputFormat)
    {
        using var bitmap = new Bitmap(1000, 1000);

        sut.Save(bitmap, Filename, outputFormat);

        File.Exists($"{Filename}.{outputFormat}").Should().BeTrue();
    }

    [Test]
    public void Save_ShouldReturnFail_WhenFormatIsNotSupported()
    {
        const string filename = $"{nameof(ImageSaver)}Test";
        const string format = "unknown";
        using var bitmap = new Bitmap(100, 100);

        var actual = sut.Save(bitmap, filename, format);

        actual.IsSuccess.Should().BeFalse();
        actual.Error.Should().Be($"Acceptable formats: {string.Join(',', ImageSaver.ImageFormats.Keys)}, but was {format.ToLowerInvariant()}");
    }

    private static void CleanUp()
    {
        foreach (var outputFormat in ImageSaver.ImageFormats.Keys)
        {
            if (File.Exists($"{Filename}.{outputFormat}"))
                File.Delete($"{Filename}.{outputFormat}");
        }
    }
}