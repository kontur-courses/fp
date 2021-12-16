using TagsCloudContainer.Results;

namespace TagsCloudContainer.Tests.FluentAssertionsExtensions
{
    public static class ResultExtension
    {
        public static ResultAssertions<T> Should<T>(this Result<T> instance)
        {
            return new ResultAssertions<T>(instance);
        }
    }
}