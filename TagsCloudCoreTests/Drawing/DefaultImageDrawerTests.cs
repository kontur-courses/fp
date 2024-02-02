using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using TagsCloudCore.Drawing;

namespace TagsCloudCoreTests.Drawing;

public class DefaultImageDrawerTests
{
    [TestCase("test", "<>\\")]
    [TestCase("test", "name|123")]
    [TestCase("test", "")]
    [TestCase("", "filename")]
    [TestCase("  ", "filename")]
    [TestCase(@"\:\", "filename")]
    public void SaveImage_ReturnsFailedResult_OnInvalidParameters(string dirPath, string filename)
    {
        var result = DefaultImageDrawer.SaveImage(new Bitmap(1, 1), dirPath, filename, ImageFormat.Png);

        result.IsSuccess
            .Should()
            .BeFalse();
    }
}