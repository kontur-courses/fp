using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloudContainer.Core;
using TagCloudContainer.Configs;
using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Core.Models;
using TagCloudContainer.Forms.Validators;

namespace TagCloudContainer.Tests;

[TestFixture]
public class SelectedValuesValidator_Should
{
    [Test]
    public void Validate_CorrectSelectedValues_ShouldBeOk()
    {
        var selectedValuesValidator = new SelectedValuesValidator();
        var availableColor = Colors.GetAll().First().Value;

        var correctSelectedValues = new SelectedValues()
        {
            WordsColor = availableColor,
            BackgroundColor = availableColor,
            FontFamily = "Arial",
            PlaceWordsRandomly = false,
            ImageSize = new Size(10, 10)
        };

        selectedValuesValidator
            .Validate(correctSelectedValues)
            .IsSuccess
            .Should()
            .BeTrue();
    }
    
    [Test]
    public void Validate_SelectedValuesWithEmptyImageSize_ShouldBeFail()
    {
        var selectedValuesValidator = new SelectedValuesValidator();
        var availableColor = Colors.GetAll().First().Value;

        var selectedValuesWithEmptyImageSize = new SelectedValues()
        {
            WordsColor = availableColor,
            BackgroundColor = availableColor,
            FontFamily = "Arial",
            PlaceWordsRandomly = false,
            ImageSize = Size.Empty
        };

        selectedValuesValidator
            .Validate(selectedValuesWithEmptyImageSize)
            .Should()
            .BeEquivalentTo(Result.Fail<ISelectedValues>("Form size can't be empty or null"));
    }
    
    [Test]
    public void Validate_SelectedValuesWithInvalidFontFamily_ShouldBeFail()
    {
        var selectedValuesValidator = new SelectedValuesValidator();
        var availableColor = Colors.GetAll().First().Value;

        var selectedValuesWithEmptyImageSize = new SelectedValues()
        {
            WordsColor = availableColor,
            BackgroundColor = availableColor,
            FontFamily = new Guid().ToString("N"),
            PlaceWordsRandomly = false,
            ImageSize = new Size(10, 10)
        };

        selectedValuesValidator
            .Validate(selectedValuesWithEmptyImageSize)
            .Should()
            .BeEquivalentTo(Result.Fail<ISelectedValues>("Incorrect font family"));
    }
}