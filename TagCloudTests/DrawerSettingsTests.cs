using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class DrawerSettingsTests
{
    [Test]
    public void Create_ReturnSuccess_OnBlankParameters()
    {
        var size = new Size(100, 100);

        var result = DrawerSettings.Create(size);

        result.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void Create_ReturnSuccess_OnCorrectParameters()
    {
        var size = new Size(100, 100);

        var result = DrawerSettings.Create(size,
            10, 30,
            "Red", "Green",
            "Times New Roman");

        result.IsSuccess.Should().BeTrue();
    }

    [TestCase(0, 1, TestName = "{m}WidthIsZero")]
    [TestCase(-1, 1, TestName = "{m}WidthIsNegative")]
    [TestCase(1, 0, TestName = "{m}HeightIsZero")]
    [TestCase(1, -1, TestName = "{m}HeightIsNegative")]
    [TestCase(0, 0, TestName = "{m}IsZero")]
    [TestCase(-1, -1, TestName = "{m}IsNegative")]
    public void Create_ReturnCorrectFail_OnSize(int width, int height)
    {
        var size = new Size(width, height);

        var result = DrawerSettings.Create(size);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Subject.Message.Should().Be($"Width and height of the image must be positive, but {size}.");
    }

    [TestCase(0, TestName = "{m}IsZero")]
    [TestCase(-1, TestName = "{m}IsNegative")]
    public void Create_ReturnCorrectFail_OnMinFontsize(int minFontSize)
    {
        var size = new Size(10, 10);

        var result = DrawerSettings.Create(size, minFontSize);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Subject.Message.Should().Be(
                $"MinFontSize should be greater than 0 and less than MaxFontSize, but {minFontSize}.");
    }

    [TestCase(1, 0, TestName = "{m}IsZero")]
    [TestCase(1, -1, TestName = "{m}IsNegative")]
    [TestCase(2, 1, TestName = "{m}LessThenMinFontSize")]
    public void Create_ReturnCorrectFail_OnMaxFontsize(int minFontSize, int maxFontSize)
    {
        var size = new Size(10, 10);

        var result = DrawerSettings.Create(size, minFontSize, maxFontSize);

        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(2);
        result.Errors[0].Message.Should().Be(
            $"MinFontSize should be greater than 0 and less than MaxFontSize, but {minFontSize}.");
        result.Errors[1].Message.Should().Be(
            $"MaxFontSize should be greater than 0 and MinFontSize, but {maxFontSize}.");
    }
    
    [Test]
    public void Create_ReturnCorrectFail_OnInvalidTextColor()
    {
        var size = new Size(10, 10);
        var invalidColorName = TestContext.CurrentContext.Test.Name;

        var result = DrawerSettings.Create(size, textColorName: invalidColorName);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Subject.Message.Should().Be($"Unknown color '{invalidColorName}'.");
    }
    
    [Test]
    public void Create_ReturnCorrectFail_OnInvalidBackgroundColor()
    {
        var size = new Size(10, 10);
        var invalidColorName = TestContext.CurrentContext.Test.Name;

        var result = DrawerSettings.Create(size, backgroundColorName: invalidColorName);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Subject.Message.Should().Be($"Unknown color '{invalidColorName}'.");
    }
    
    [Test]
    public void Create_ReturnCorrectFail_OnInvalidFontFamily()
    {
        var size = new Size(10, 10);
        var invalidFontFamilyName = TestContext.CurrentContext.Test.Name;

        var result = DrawerSettings.Create(size, fontFamilyName: invalidFontFamilyName);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Subject.Message.Should().Be($"Font '{invalidFontFamilyName}' cannot be found.");
    }
}