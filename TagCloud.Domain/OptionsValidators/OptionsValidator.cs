using System.Drawing;

public class OptionsValidator : IOptionsValidator
{
    public Result<None> ValidateOptions(DomainOptions domainOpitons)
    {
        var result = Result.Fail<None>("");
        var renderOptions = domainOpitons.RenderOptions;
        var tagCloudOptions = domainOpitons.TagCloudOptions;
        var wordExtractionOptions = domainOpitons.WordExtractionOptions;

        if (renderOptions.ImageSize.Width <= 0 || renderOptions.ImageSize.Height <= 0)
            result = result.RefineError($"Invalid image size {renderOptions.ImageSize}");

        if (renderOptions.MaxFontSize <= renderOptions.MinFontSize)
            result = result.RefineError("Minimal font size is greater than maximal font size or equal");

        if (tagCloudOptions.MaxTagsCount < -1)
            result = result.RefineError("Invalid maximal tags count");

        if (!IsPointInsideSize(tagCloudOptions.Center, renderOptions.ImageSize))
            result = result.RefineError($"Tag cloud center is outside of an image");

        if (wordExtractionOptions.MinWordLength < 0)
            result = result.RefineError("Invaild minimal word length");

        return result.Error == "" ? Result.Ok() : result;
    }

    private bool IsPointInsideSize(Point p, Size s)
    {
        return p.X >= 0 && p.Y >= 0 && p.X < s.Width && p.Y < s.Height;
    }
}
