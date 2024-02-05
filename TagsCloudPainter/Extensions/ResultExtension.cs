using ResultLibrary;

namespace TagsCloudPainter.Extensions;

public static class ResultExtension
{
    public static Result<dynamic> GetFirstFailedResult(params Result<dynamic>[] results)
    {
        return results.FirstOrDefault(result => !result.IsSuccess);
    }
}
