using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests;

[TestFixture]
public class ResultTest
{
    [Test]
    public void Create_Ok()
    {
        var r = Result.Ok(42);
        r.IsSuccess.Should().BeTrue();
        r.GetValueOrThrow().Should().Be(42);
    }

    [Test]
    public void Create_Fail()
    {
        var r = Result.Fail<int>("123");

        r.IsSuccess.Should().BeFalse();
        r.Error.Should().Be("123");
    }

    [Test]
    public void ReturnsFail_FromResultOf_OnException()
    {
        var res = Result.Of<int>(() => { throw new Exception("123"); });

        res.Should().BeEquivalentTo(Result.Fail<int>("123"));
    }

    [Test]
    public void ReturnsFailWithCustomMessage_FromResultOf_OnException()
    {
        var res = Result.Of<int>(() => { throw new Exception("123"); }, "42");

        res.Should().BeEquivalentTo(Result.Fail<int>("42"));
    }

    [Test]
    public void ReturnsOk_FromResultOf_WhenNoException()
    {
        var res = Result.Of(() => 42);

        res.Should().BeEquivalentTo(Result.Ok(42));
    }

    [Test]
    public void RunThen_WhenOk()
    {
        var res = Result.Ok(42)
            .Then(n => n + 10);
        res.Should().BeEquivalentTo(Result.Ok(52));
    }
    
    [Test]
    public void RunThen_WhenContinuationIsOk()
    {
        var res = Result.Ok(42)
            .Then(n => Result.Ok(n + 10));
        res.Should().BeEquivalentTo(Result.Ok(52));
    }

    [Test]
    public void SkipThen_WhenFail()
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
    public void Then_ReturnsFail_OnException()
    {
        Func<int, int> continuation = n =>
        {
            throw new Exception("123");
        };
        var res = Result.Ok(42)
            .Then(continuation);
        res.Should().BeEquivalentTo(Result.Fail<int>("123"));
    }
    
    [Test]
    public void Then_ReturnsFail_OnFailedContinuation()
    {
        Func<int, Result<int>> continuation = n => Result.Fail<int>("123");
        var res = Result.Ok(42)
            .Then(continuation);
        res.Should().BeEquivalentTo(Result.Fail<int>("123"));
    }

    [Test]
    public void RunThen_WhenOk_Scenario()
    {
        var res =
            Result.Ok("1358571172")
                .Then(int.Parse)
                .Then(i => Convert.ToString(i, 16))
                .Then(hex => Guid.Parse(hex + hex + hex + hex));
        res.Should().BeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
    }

    [Test]
    public void ReplaceError_IfFail()
    {
        Result.Fail<string>("error")
            .ReplaceError(e => "replaced")
            .Should().BeEquivalentTo(Result.Fail<string>("replaced"));
    }

    [Test]
    public void ReplaceError_DoNothing_IfSuccess()
    {
        Result.Ok(42)
            .ReplaceError(e => "replaced")
            .Should().BeEquivalentTo(Result.Ok(42));
    }

    [Test]
    public void ReplaceError_DontReplace_IfCalledBeforeError()
    {
        Result.Ok(42)
            .ReplaceError(e => "replaced")
            .Then(n => Result.Fail<int>("error"))
            .Should().BeEquivalentTo(Result.Fail<int>("error"));
    }

    [Test]
    public void RefineError_AddErrorMessageBeforePreviousErrorText()
    {
        var calculation = Result.Fail<string>("No connection");
        calculation
            .RefineError("Posting results to db")
            .Should().BeEquivalentTo(Result.Fail<string>("Posting results to db. No connection"));
    }
}