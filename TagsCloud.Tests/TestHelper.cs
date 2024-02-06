using FluentAssertions;
using TagsCloud.Results;

namespace TagsCloud.Tests;

public static class TestHelper
{
    public static void AssertResultSuccess<T>(Result<T> result)
    {
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    public static void AssertResultFail<T>(Result<T> result)
    {
        AssertResultFailAndErrorText(result);
    }

    public static void AssertResultFailAndErrorText<T>(Result<T> result, string? errorPart = null)
    {
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();

        if (errorPart is not null)
            result.Error.Should().Contain(errorPart);
    }
}