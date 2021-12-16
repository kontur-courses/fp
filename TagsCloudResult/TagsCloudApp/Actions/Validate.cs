using System;
using TagsCloudContainer.Results;

namespace TagsCloudApp.Actions
{
    public static class Validate
    {
        public static Result<T> Positive<T>(string valueName, T value) where T : IComparable<T>
        {
            return value.CompareTo(default) > 0
                ? value
                : Result.Fail<T>($"{valueName} must be positive.");
        }
    }
}