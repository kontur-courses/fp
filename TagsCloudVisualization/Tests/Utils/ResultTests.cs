using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization.Tests.Utils
{
    [TestFixture]
    public class ResultTests
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
        public void ResultOf_ReturnsFail_OnException()
        {
            var res = Result.Of<int>(() => throw new Exception("123"));
            res.Should().BeEquivalentTo(Result.Fail<int>("123"));
        }

        [Test]
        public void ResultOf_ReturnsFailWithCustomMessage_OnException()
        {
            var res = Result.Of<int>(() => throw new Exception("123"), "42");
            res.Should().BeEquivalentTo(Result.Fail<int>("42"));
        }

        [Test]
        public void ResultOf_ReturnsOk_WhenNoException()
        {
            var res = Result.Of(() => 42);

            res.Should().BeEquivalentTo(Result.Ok(42));
        }

        [Test]
        public void Then_ReturnsOk_WhenOk()
        {
            var res = Result.Ok(42)
                .Then(n => n + 10);
            res.Should().BeEquivalentTo(Result.Ok(52));
        }

        [Test]
        public void Then_ReturnsOk_WhenContinuationIsOk()
        {
            var res = Result.Ok(42)
                .Then(n => Result.Ok(n + 10));
            res.Should().BeEquivalentTo(Result.Ok(52));
        }

        [Test]
        public void Then_IsNotCalled_WhenFail()
        {
            var fail = Result.Fail<int>("test");
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
            int Continuation(int n) => throw new Exception("123");

            var res = Result.Ok(42)
                .Then(Continuation);
            res.Should().BeEquivalentTo(Result.Fail<int>("123"));
        }

        [Test]
        public void Then_ReturnsFail_OnFailedContinuation()
        {
            Result<int> Continuation(int n) => Result.Fail<int>("123");

            var res = Result.Ok(42)
                .Then(Continuation);
            res.Should().BeEquivalentTo(Result.Fail<int>("123"));
        }

        [Test]
        public void OnFail_IsCalled_WhenFail()
        {
            var fail = Result.Fail<int>("test");
            var errorHandler = A.Fake<Action<string>>();

            var res = fail.OnFail(errorHandler);

            A.CallTo(() => errorHandler(null)).WithAnyArguments().MustHaveHappened();
            res.Should().BeEquivalentTo(fail);
        }

        [Test]
        public void OnFail_IsNotCalled_WhenOk()
        {
            var ok = Result.Ok(42);

            var res = ok.OnFail(v => { Assert.Fail("Should not be called"); });

            res.Should().BeEquivalentTo(ok);
        }

        [Test]
        public void Then_ReturnsOk_OnOkScenario()
        {
            var res =
                Result.Ok("1358571172")
                    .Then(int.Parse)
                    .Then(i => Convert.ToString(i, 16))
                    .Then(hex => Guid.Parse(hex + hex + hex + hex));
            res.Should().BeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void Then_ReturnsOk_OnComplexScenario()
        {
            var parsed = Result.Ok("1358571172").Then(int.Parse);
            var res = parsed
                .Then(i => Convert.ToString(i, 16))
                .Then(hex => parsed.GetValueOrThrow() + " -> " + Guid.Parse(hex + hex + hex + hex));
            res.Should().BeEquivalentTo(Result.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
        }

        [Test]
        public void ReplaceError_WorksCorrectly_WhenFail()
        {
            Result.Fail("Test error")
                .ReplaceError(e => "replaced")
                .Should().BeEquivalentTo(Result.Fail("replaced"));
        }

        [Test]
        public void ReplaceError_DoNothing_WhenOk()
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
            var calculation = Result.Fail("No connection");
            calculation
                .RefineError("Posting results to db")
                .Should().BeEquivalentTo(Result.Fail("Posting results to db. No connection"));
        }

    }
}