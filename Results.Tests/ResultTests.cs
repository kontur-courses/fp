using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ResultExtensions;
using ResultOf;
using System;

namespace Results.Tests;

[TestFixture]
public class ResultTests
{
    [Test]
    public void ShouldCreate_Ok()
    {
        var r = Result.Ok(42);
        r.IsSuccess.Should().BeTrue();
        r.GetValueOrThrow().Should().Be(42);
    }

    [Test]
    public void ShouldCreate_Fail()
    {
        var r = Result.Fail<int>("123");

        r.IsSuccess.Should().BeFalse();
        r.Error.Should().Be("123");
    }

    [Test]
    public void ShouldReturnFail_FromResultOf_OnException()
    {
        var res = Result.Of<int>(() => { throw new Exception("123"); });

        res.Should().BeEquivalentTo(Result.Fail<int>("123"));
    }

    [Test]
    public void ShouldReturnFailWithCustomMessage_FromResultOf_OnException()
    {
        var res = Result.Of<int>(() => { throw new Exception("123"); }, "42");

        res.Should().BeEquivalentTo(Result.Fail<int>("42"));
    }

    [Test]
    public void ShouldReturnOk_FromResultOf_WhenNoException()
    {
        var res = Result.Of(() => 42);

        res.Should().BeEquivalentTo(Result.Ok(42));
    }

    [Test]
    public void ShouldRunThen_WhenOk()
    {
        var res = Result.Ok(42)
            .Then(n => n + 10);
        res.Should().BeEquivalentTo(Result.Ok(52));
    }

    [Test]
    public void ShouldRunThen_WhenContinuationIsOk()
    {
        var res = Result.Ok(42)
            .Then(n => Result.Ok(n + 10));
        res.Should().BeEquivalentTo(Result.Ok(52));
    }

    [Test]
    public void ShouldSkipThen_WhenFail()
    {
        var fail = Result.Fail<int>("������");
        var called = false;
        fail.Then(n =>
        {
            called = true;
            return n;
        });
        called.Should().BeFalse();
    }

    [Test]
    public void Then_ShouldReturnFail_OnException()
    {
        Func<int, int> continuation = n => throw new Exception("123");
        var res = Result.Ok(42)
            .Then(continuation);
        res.Should().BeEquivalentTo(Result.Fail<int>("123"));
    }

    [Test]
    public void Then_ShouldReturnFail_OnFailedContinuation()
    {
        Func<int, Result<int>> continuation = n => Result.Fail<int>("123");
        var res = Result.Ok(42)
            .Then(continuation);
        res.Should().BeEquivalentTo(Result.Fail<int>("123"));
    }

    [Test]
    public void ShouldRunOnFail_WhenFail()
    {
        var fail = Result.Fail<int>("������");
        var errorHandler = A.Fake<Action<string>>();

        var res = fail.OnFail(errorHandler);

        A.CallTo(() => errorHandler(null!)).WithAnyArguments().MustHaveHappened();
        res.Should().BeEquivalentTo(fail);
    }

    [Test]
    public void ShouldSkipOnFail_WhenOk()
    {
        var ok = Result.Ok(42);

        var res = ok.OnFail(v => { Assert.Fail("Should not be called"); });

        res.Should().BeEquivalentTo(ok);
    }

    [Test]
    public void ShouldRunThen_WhenOk_OnSimpleScenario()
    {
        var res =
            Result.Ok("1358571172")
                .Then(int.Parse)
                .Then(i => Convert.ToString(i, 16))
                .Then(hex => Guid.Parse(hex + hex + hex + hex));
        res.Should().BeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
    }

    [Test]
    public void ShouldRunThen_WhenOk_OnComplexScenario()
    {
        var parsed = Result.Ok("1358571172").Then(int.Parse);
        var res = parsed
            .Then(i => Convert.ToString(i, 16))
            .Then(hex => parsed.GetValueOrThrow() + " -> " + Guid.Parse(hex + hex + hex + hex));
        res.Should().BeEquivalentTo(Result.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
    }
}