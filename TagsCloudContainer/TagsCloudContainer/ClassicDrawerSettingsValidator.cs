using System.Drawing;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class ClassicDrawerSettingsValidator : IValidator<ClassicDrawerSettings>
{
    public Result Validate(ClassicDrawerSettings value)
    {
        return Result.SuccessIf(() => !value.ImageRectangle.IsEmpty, "Image rectangle is empty.")
            .BindIf(value.WriteNumbers, () =>
                Result.SuccessIf(() => value.ImageRectangle.Width > 0,
                        "Width of image rectangle is less than or equal zero.")
                    .CombineCheck(() => value.ImageRectangle.Height > 0,
                        "Height of image rectangle is less than or equal zero.")
                    .CombineCheck(() => value.NumbersSize > 0, "Numbers size is less than or equal zero.")
                    // TODO: Now i check by trying to create font family, because we cannot say in moment: yes this is right font family name, this is installed on pc and TrueType font. 
                    .CombineCheck(() => Result.Try(() => new FontFamily(value.NumbersFamily)),
                        $"Invalid value for numbers font family: '{value.NumbersFamily}'.")
                    .CombineCheck(
                        () => value.NumbersBrush is not SolidBrush brush || !brush.Color.IsEmpty,
                        "Numbers brush is using empty color"))
            .CombineCheck(() => Result.Try(() => new FontFamily(value.TextFontFamily)),
                $"Invalid value for text font family: '{value.TextFontFamily}'.")
            .CombineCheck(() => value.TextBrush is not SolidBrush brush || !brush.Color.IsEmpty,
                "Text brush is using empty color")
            .CombineCheck(() => value.MinimumTextFontSize > 0, "Minimum text font size is less than or equal zero.")
            .CombineCheck(() => value.MaximumTextFontSize > 0, "Maximum text font size is less than or equal zero.")
            .CombineCheck(() => value.MinimumTextFontSize <= value.MaximumTextFontSize,
                "Minimum text font size is greater maximum text font size.")
            .CombineCheck(() => value.RectangleBorderBrush is not SolidBrush brush || !brush.Color.IsEmpty,
                "Rectangle border brush is using empty color")
            .BindIf(() => value.FillRectangles, () => Result.SuccessIf(
                value.RectangleFillBrush is not SolidBrush brush || !brush.Color.IsEmpty,
                "Rectangle fill brush is using empty color"))
            .CombineCheck(() => value.BackgroundBrush is not SolidBrush brush || !brush.Color.IsEmpty,
                "Background brush is using empty color")
            .CombineCheck(() => value.RectangleBorderSize >= 0, "Rectangle border size is less than zero.");
    }
}