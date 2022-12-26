using System;
using ResultOf;

namespace TagsCloudVisualizationTests
{
    public static class ResultExtensions
    {
        public static T GetValueOrThrow<T>(this Result<T> result)
        {
            if (result.IsSuccess) return result.Value;
            throw new InvalidOperationException($"No value. Only Error {result.Error}");
        }
    }
}