using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class RandomColoredDrawerSettingsValidator : IValidator<RandomColoredDrawerSettings>
{
    public Result Validate(RandomColoredDrawerSettings value)
    {
        return Result.SuccessIf(() => !value.ImageRectangle.IsEmpty, "Image rectangle is empty.")
            .CombineCheck(() => value.ImageRectangle.Width > 0,
                "Width of image rectangle is less than or equal zero.")
            .CombineCheck(() => value.ImageRectangle.Height > 0,
                "Height of image rectangle is less than or equal zero.")
            .CombineCheck(() => value.NumbersSize > 0, "Numbers size is less than or equal zero.")
            .CombineCheck(() => value.MinimumTextFontSize > 0, "Minimum text font size is less than or equal zero.")
            .CombineCheck(() => value.MaximumTextFontSize > 0, "Maximum text font size is less than or equal zero.")
            .CombineCheck(() => value.MinimumTextFontSize <= value.MaximumTextFontSize,
                "Minimum text font size is greater maximum text font size.")
            .CombineCheck(() => value.RectangleBorderSize >= 0, "Rectangle border size is less than zero.");
    }
}