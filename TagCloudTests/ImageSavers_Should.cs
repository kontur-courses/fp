using System.Drawing;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;
using TagCloudCore.Interfaces.Settings;
using TagCloudCoreExtensions.ImageSavers;

namespace TagCloudTests;

[TestFixture]
public class ImageSavers_Should
{
    private IImagePathSettingsProvider _pathSettingsProvider = null!;
    private Image _image = null!;
    private string? _imagePath;

    [SetUp]
    public void Setup()
    {
        var pathSettings = A.Fake<IImagePathSettings>();
        _pathSettingsProvider = A.Fake<IImagePathSettingsProvider>();
        A.CallTo(() => _pathSettingsProvider.GetImagePathSettings())
            .Returns(pathSettings);
        _image = new Bitmap(800, 600);
    }

    [TearDown]
    public void TearDown()
    {
        _image.Dispose();
        RemoveFileIfExists();
    }

    [TestCaseSource(nameof(ImagesFormatsTestCaseData))]
    public void SaveImage_Successfully(Func<IImagePathSettingsProvider, IImageSaver> saverProvider)
    {
        var saver = saverProvider(_pathSettingsProvider);
        _imagePath = $"image{saver.SupportedExtension}";
        RemoveFileIfExists();
        _pathSettingsProvider.GetImagePathSettings().ImagePath = _imagePath;
        saver.SaveImage(_image);

        File.Exists(_imagePath).Should().BeTrue();
    }

    private static TestCaseData[] ImagesFormatsTestCaseData =
    {
        new TestCaseData(
            new Func<IImagePathSettingsProvider, IImageSaver>(provider => new PngImageSaver(provider))
        ).SetName("Png"),
        new TestCaseData(
            new Func<IImagePathSettingsProvider, IImageSaver>(provider => new JpegImageSaver(provider))
        ).SetName("Jpeg"),
        new TestCaseData(
            new Func<IImagePathSettingsProvider, IImageSaver>(provider => new GifImageSaver(provider))
        ).SetName("Gif"),
        new TestCaseData(
            new Func<IImagePathSettingsProvider, IImageSaver>(provider => new EmfImageSaver(provider))
        ).SetName("Emf")
    };

    [Test]
    public void Fail_ForBadExtension()
    {
        _imagePath = "image.png";

        _pathSettingsProvider.GetImagePathSettings().ImagePath = _imagePath;

        var result = new JpegImageSaver(_pathSettingsProvider).SaveImage(_image);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Wrong file extension: .png");
    }

    private void RemoveFileIfExists()
    {
        if (_imagePath is not null && File.Exists(_imagePath))
            File.Delete(_imagePath);
    }
}