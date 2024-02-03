using Aspose.Drawing;
using Aspose.Drawing.Imaging;
using TagCloud.Utils.Extensions;

namespace TagCloud.Tests;

public class UtilsExtensionsTests
{
    [Test]
    public void ConvertToImageFormat_ReturnsFalseOnIncorrectFormat()
    {
        var convertedResult = "abcd".ConvertToImageFormat();

        convertedResult.IsSuccess
            .Should()
            .BeFalse();

        convertedResult.Error
            .Should()
            .Be("Формат abcd недоступен");
    } 
    
    [Test]
    public void ConvertToImageFormat_ParsesCorrectly()
    {
        
        var convertedResult = "png".ConvertToImageFormat();

        convertedResult.IsSuccess
            .Should()
            .BeTrue();

        convertedResult.Value
            .Should()
            .Be(ImageFormat.Png);
    }
    
    [Test]
    public void ParseColor_ReturnsFalseOnIncorrectScheme()
    {
        var parseResult = (300, 300, 300).ParseColor();

        parseResult.IsSuccess
            .Should()
            .BeFalse();
    }
    
    [Test]
    public void ParseColor_ParsesColorCorrectly()
    {
        var parseResult = (000, 111, 222).ParseColor();
        
        parseResult.IsSuccess
            .Should()
            .BeTrue();
    
        parseResult.Value
            .Should()
            .Be(Color.FromArgb(255, 0, 111, 222));
    }
}