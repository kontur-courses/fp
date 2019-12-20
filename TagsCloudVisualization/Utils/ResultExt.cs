using System;

namespace TagsCloudVisualization.Utils
{
    public static class ResultExt
    {
        public static Result<T> AsResult<T>(this T value)
        {
            return Result.Ok(value);
        }
    }
}
