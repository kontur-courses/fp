using FluentAssertions;
using TagsCloud.Results;

namespace TagsCloud.Tests;

public static class TestHelper
{
    public static void AssertResultFail<T>(Result<T> result)
    {
        AssertResultFailAndErrorText(result);
    }

    public static void AssertResultFailAndErrorText<T>(Result<T> result, string? errorPart = null)
    {
        result.IsSuccess.Should().Be(false);
        result.Error.Should().NotBeNull();

        if (errorPart is not null)
            result.Error.Should().Contain(errorPart);
    }
}