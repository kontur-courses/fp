using System.Drawing.Text;
using TagCloudContainer.Core;
using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Forms.Interfaces;

namespace TagCloudContainer.Forms.Validators;

public class SelectedValuesValidator : IConfigValidator<ISelectedValues>
{
    public Result<ISelectedValues> Validate(ISelectedValues selectedValues)
    {
        var systemFonts = new InstalledFontCollection().Families.Select(f => f.Name);
        
        if (selectedValues.ImageSize.IsEmpty || selectedValues.ImageSize == null) 
            return Result.Fail<ISelectedValues>("Form size can't be empty or null");
        if (!systemFonts.Contains(selectedValues.FontFamily)) 
            return Result.Fail<ISelectedValues>("Incorrect font family");
        
        return Result.Ok(selectedValues);
    }
}