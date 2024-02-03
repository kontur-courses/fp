using System.Drawing;
using TagCloudTests.TestData;
using ColorConverter = TagCloud.Extensions.ColorConverter;

namespace TagCloudTests;

public class ColorConverterTests
{
    [TestCaseSource(typeof(ColorConverterTestData), nameof(ColorConverterTestData.RightCases))]
    public void Converter_ShouldConvertStringToColor_WhenStringInRightFormat(string hexString, Color expectedColor)
    {
        ColorConverter.TryConvert(hexString, out var color)
            .Should()
            .BeTrue();
        color
            .Should()
            .Be(expectedColor);
    }
    
    [TestCaseSource(typeof(ColorConverterTestData), nameof(ColorConverterTestData.WrongCases))]
    public void Converter_ShouldFail_WhenWrongStringFormat(string hexString)
    {
        ColorConverter.TryConvert(hexString, out var color).Should().BeFalse();
    }
}