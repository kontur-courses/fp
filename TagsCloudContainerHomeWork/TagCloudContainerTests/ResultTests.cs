using System;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainerCore.Result;

namespace TagCloudContainerTests;

[TestFixture]
public class ResultTests
{
    [Test]
    public void Result_WhenWasException_ShouldNotThrow()
    {
        Result<int> result = 1;
        var zero = 0;

        Assert.DoesNotThrow(() => { result.Then(x => x / zero); });
    }

    [Test]
    public void GetValueOrThrow_WhenWasException_ShouldThrow()
    {
        Result<int> result = 1;
        var zero = 0;

        Assert.Throws<InvalidOperationException>(() => { result.Then(x => x / zero).GetValueOrThrow(); });
    }

    [Test]
    public void IsSuccess_WhenWasException_ShouldBeFalse()
    {
        var result = ResultExtension.Ok(1);
        var zero = 0;

        result = result.Then(x => x / zero);

        result.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void IsSuccess_WhenWasNotException_ShouldBeTrue()
    {
        var result = ResultExtension.Ok(10);

        result = result.Then(x => x / 10);

        result.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void Error_WhenWasException_ShouldNotBeNullOrEmpty()
    {
        var result = ResultExtension.Ok(1);
        var zero = 0;

        result = result.Then(x => x / zero);

        result.Error.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public void Error_WhenWasNotException_ShouldBeNullOrEmpty()
    {
        var result = ResultExtension.Ok(10);

        result = result.Then(x => x / 10);

        result.Error.Should().BeNullOrWhiteSpace();
    }
}