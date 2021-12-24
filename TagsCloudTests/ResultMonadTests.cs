using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ResultMonad;
using ResultMonad.Extensions;

namespace TagsCloudTests
{
    [TestFixture]
    public class ResultMonadTests
    {
        [Test]
        public void Create_Ok()
        {
            var actual = Result.Ok(42);
            
            actual.IsSuccess.Should().BeTrue();
            actual.GetValueOrThrow().Should().Be(42);
        }

        [Test]
        public void Create_Fail()
        {
            var actual = Result.Fail<int>("123");
            
            actual.IsSuccess.Should().BeFalse();
            actual.Error.Should().Be("123");
        }

        [Test]
        public void ReturnsFail_FromResultOf_OnException()
        {
            var actual = Result.Of<int>(() => throw new Exception("123"));

            actual.Should().BeEquivalentTo(Result.Fail<int>("123"));
        }

        [Test]
        public void ReturnsFailWithCustomMessage_FromResultOf_OnException()
        {
            var actual = Result.Of<int>(() => throw new Exception("123"), "42");

            actual.Should().BeEquivalentTo(Result.Fail<int>("42"));
        }

        [Test]
        public void ReturnsOk_FromResultOf_WhenNoException()
        {
            var actual = Result.Of(() => 42);

            actual.Should().BeEquivalentTo(Result.Ok(42));
        }

        [Test]
        public void RunThen_WhenOk()
        {
            var actual = Result.Ok(42)
                .Then(n => n + 10);
            
            actual.Should().BeEquivalentTo(Result.Ok(52));
        }
        
        [Test]
        public void RunThen_WhenContinuationIsOk()
        {
            var actual = Result.Ok(42)
                .Then(n => Result.Ok(n + 10));
            
            actual.Should().BeEquivalentTo(Result.Ok(52));
        }

        [Test]
        public void SkipThen_WhenFail()
        {
            var fail = Result.Fail<int>("Error message");
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
            var continuation = new Func<int, int>(n => throw new Exception("123"));
            
            var actual = Result.Ok(42)
                .Then(continuation);
            
            actual.Should().BeEquivalentTo(Result.Fail<int>("123"));
        }
        
        [Test]
        public void Then_ReturnsFail_OnFailedContinuation()
        {
            var continuation = new Func<int, Result<int>>(_ => Result.Fail<int>("123"));
            
            var actual = Result.Ok(42)
                .Then(continuation);
            
            actual.Should().BeEquivalentTo(Result.Fail<int>("123"));
        }

        [Test]
        public void RunOnFail_WhenFail()
        {
            var fail = Result.Fail<int>("fail");
            var errorHandler = A.Fake<Action<string>>();

            var actual = fail.OnFail(errorHandler);

            A.CallTo(() => errorHandler(null)).WithAnyArguments().MustHaveHappened();
            actual.Should().BeEquivalentTo(fail);
        }

        [Test]
        public void SkipOnFail_WhenOk()
        {
            var ok = Result.Ok(42);

            var actual = ok.OnFail(v => { Assert.Fail("Should not be called"); });

            actual.Should().BeEquivalentTo(ok);
        }

        [Test]
        public void RunThen_WhenOk_Scenario()
        {
            var actual =
                Result.Ok("1358571172")
                    .Then(int.Parse)
                    .Then(i => Convert.ToString(i, 16))
                    .Then(hex => Guid.Parse(hex + hex + hex + hex));
            
            actual.Should().BeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void RunThen_WhenOk_ComplexScenario()
        {
            var parsed = Result.Ok("1358571172").Then(int.Parse);
            
            var actual = parsed
                .Then(i => Convert.ToString(i, 16))
                .Then(hex => parsed.GetValueOrThrow() + " -> " + Guid.Parse(hex + hex + hex + hex));
            
            actual.Should().BeEquivalentTo(Result.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
        }

        [Test]
        public void ReplaceError_IfFail()
        {
            Result.Fail<None>("error")
                .ReplaceError(e => "replaced")
                .Should().BeEquivalentTo(Result.Fail<None>("replaced"));
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
            var calculation = Result.Fail<None>("No connection");
            calculation
                .RefineError("Posting results to db")
                .Should().BeEquivalentTo(Result.Fail<None>("Posting results to db. No connection"));
        }
    }
}