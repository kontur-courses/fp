﻿using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests;

public class CustomOptionsValidatorTests
{
    private CustomOptions options;

    [SetUp]
    public void Setup()
    {
        options = new CustomOptions
        {
            WorkingDir = $"{Path.Combine(Directory.GetCurrentDirectory(), "WorkingDir")}",
            WordsFileName = "SmallText.txt",
            BoringWordsName = "SmallText.txt",
            Font = "Arial",
            PictureSize = 600,
            MinTagSize = 15,
            MaxTagSize = 30,
            BackgroundColor = "White",
            FontColor = "Blue",
            ImageFormat = "png"
        };
    }

    [Test]
    public void ValidateConfig_AddPreSetOptions_ShouldReturnValidResult()
    {
        var result = CustomOptionsValidator.ValidateOptions(options);

        result.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void ValidateConfig_AddLowerCaseColor_ShouldReturnValidResult()
    {
        options.FontColor = "white";

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void ValidateConfig_AddEmptyTextsPath_ShouldReturnInvalidResult()
    {
        options.WorkingDir = "";

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Texts directory does not exist");
        result.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void ValidateConfig_AddEmptyWordsFileName_ShouldReturnInvalidResult()
    {
        options.WordsFileName = "";

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Tag file does not exist");
        result.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void ValidateConfig_AddEmptyBoringWordsFileName_ShouldReturnInvalidResult()
    {
        options.BoringWordsName = "";

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Exclude words file does not exist");
        result.IsSuccess.Should().BeFalse();
    }

    [TestCase("")]
    [TestCase("NonExistingFont")]
    public void ValidateConfig_AddIncorectFontName_ShouldReturnInvalidResult(string font)
    {
        options.Font = font;

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be($"Font \"{font}\" can't be found");
        result.IsSuccess.Should().BeFalse();
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void ValidateConfig_AddPictureSizeLessThanOne_ShouldReturnInvalidResult(int size)
    {
        options.PictureSize = size;

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Picture size should be above 0");
        result.IsSuccess.Should().BeFalse();
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void ValidateConfig_AddMaxFontMoreThamPictureSize_ShouldReturnInvalidResult(int size)
    {
        options.MinTagSize = size;

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Font size should be above 0");
        result.IsSuccess.Should().BeFalse();
    }

    [TestCase("")]
    [TestCase("NonExistingColor")]
    public void ValidateConfig_AddIncorrectFontColorName_ShouldReturnInvalidResult(string font)
    {
        options.FontColor = font;

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Invalid font color");
        result.IsSuccess.Should().BeFalse();
    }

    [TestCase("")]
    [TestCase("NonExistingColor")]
    public void ValidateConfig_AddIncorrectBackgroundColorName_ShouldReturnInvalidResult(string font)
    {
        options.BackgroundColor = font;

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Invalid backgroud color");
        result.IsSuccess.Should().BeFalse();
    }

    [TestCase(600)]
    [TestCase(601)]
    public void ValidateConfig_AddFontSizeMoreOrEqualThanPictureSize_ShouldReturnInvalidResult(
        int size)
    {
        options.MaxTagSize = size;

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Font size should be less than picture size");
        result.IsSuccess.Should().BeFalse();
    }

    [TestCase("png")]
    [TestCase("PNG")]
    public void ValidateConfig_AddSupportedFormat_ShouldReturnValidResult(string format)
    {
        options.ImageFormat = format;

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void ValidateConfig_AddUsupportedFormat_ShouldReturnInvalidResult()
    {
        options.ImageFormat = "ping";

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Unsupported image format");
        result.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void ValidateConfig_AddDirectoryWithoutMystem_ShouldReturnInvalidResult()
    {
        options.WorkingDir = "c:\\Windows\\System32";

        var result = CustomOptionsValidator.ValidateOptions(options);

        result.Exception!.Message.Should().Be("Mystem not found in working directory");
        result.IsSuccess.Should().BeFalse();
    }
}