using TagCloud.FileSaver;

namespace TagCloudTests;

[TestFixture]
public class ImageSaver_Should
{
    private ISaver sut = new ImageSaver();
    private Bitmap bitmap;
    private const string filename = "test";
    private const string extension = "png";
    private const string badFormat = "\n";

    [SetUp]
    public void SetUp()
    {
        bitmap = new Bitmap(50, 50);
    }


    [Test]
    public void SaveBitmapToFile()
    {
        var result = sut.Save(bitmap, filename, extension);

        result.IsSuccess.Should().BeTrue();
        File.Exists($"{filename}.{extension}").Should().BeTrue();

        File.Delete($"{filename}.{extension}");
    }

    [Test]
    public void ReturnFail_WhenFormatIsNotSupported()
    {
        var result = sut.Save(bitmap, filename, badFormat);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be($"{badFormat} output format is not supported");
    }
}