using TagCloudCore.Domain.Providers;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;
using TagCloudCore.Interfaces.Settings;
using TagCloudCoreExtensions.ImageSavers;

namespace TagCloudTests;

[TestFixture]
public class ImageSaverProvider_Should
{
    private IImageSaverProvider _imageSaverProvider = null!;
    private IImagePathSettings _pathSettings = null!;

    [SetUp]
    public void Setup()
    {
        _pathSettings = A.Fake<IImagePathSettings>();
        var pathSettingsProvider = A.Fake<IImagePathSettingsProvider>();
        A.CallTo(() => pathSettingsProvider.GetImagePathSettings())
            .Returns(_pathSettings);

        _imageSaverProvider = new ImageSaverProvider(
            new IImageSaver[]
            {
                new PngImageSaver(pathSettingsProvider),
                new JpegImageSaver(pathSettingsProvider),
                new GifImageSaver(pathSettingsProvider),
                new EmfImageSaver(pathSettingsProvider)
            },
            pathSettingsProvider
        );
    }

    [TestCase(".png", typeof(PngImageSaver), TestName = "Png")]
    [TestCase(".jpeg", typeof(JpegImageSaver), TestName = "Jpeg")]
    [TestCase(".gif", typeof(GifImageSaver), TestName = "Gif")]
    [TestCase(".emf", typeof(EmfImageSaver), TestName = "Emf")]
    public void ReturnCorrectSaver_ForCorrectExtension(string extension,
        Type expectedSaverType)
    {
        _pathSettings.ImagePath = $"image{extension}";
        _imageSaverProvider.GetSaver().GetValueOrThrow().GetType()
            .Should().Be(expectedSaverType);
    }

    [Test]
    public void Fail_ForBadExtension()
    {
        _pathSettings.ImagePath = "image.abc";
        var saverResult = _imageSaverProvider.GetSaver();
        saverResult.IsSuccess.Should().BeFalse();
        saverResult.Error.Should().Be("No saver for extension: .abc");
    }
}