using System.IO;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Gui;

public class GuiGraphicsProviderSettingsValidator : IValidator<GuiGraphicsProviderSettings>
{
    public Result Validate(GuiGraphicsProviderSettings value)
    {
        return Result.SuccessIf(() => value.Width > 0, "Width of images is less than or equal zero.")
            .CombineCheck(() => value.Height > 0, "Height of images is less than or equal zero.")
            .BindIf(() => value.Save, () => Result
                .SuccessIf(() => Directory.Exists(value.SavePath), "Save path is not existed directory")
                .CombineCheck(() => FileExtensions.CheckWritingAccessToDirectory(value.SavePath),
                    "Save path is not writable for current user"));
    }
}