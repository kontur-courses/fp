using TagCloudContainer.Core;
using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Forms.Interfaces;

namespace TagCloudContainer.Forms.Validators;

public class TagCloudPlacerConfigValidator : IConfigValidator<ITagCloudPlacerConfig>
{
    public Result<ITagCloudPlacerConfig> Validate(ITagCloudPlacerConfig tagCloudPlacerConfig)
    {
        if (tagCloudPlacerConfig.FieldCenter.IsEmpty || tagCloudPlacerConfig.FieldCenter == null)
            return Result.Fail<ITagCloudPlacerConfig>("Field center point can't be empty or null");
        if (tagCloudPlacerConfig.PutRectangles == null)
            return Result.Fail<ITagCloudPlacerConfig>("Put rectangles list can't be null");
        if (tagCloudPlacerConfig.NearestToTheFieldCenterPoints == null)
            return Result.Fail<ITagCloudPlacerConfig>("Nearest to the field center points list can't be null");

        return Result.Ok(tagCloudPlacerConfig);
    }    
}