using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace ErrorHandling.Solved
{
	[TestFixture]
	public class Result_Should
	{
        [Test]
        public void Create_Ok()
        {
            var r = Result.Ok(42);
            r.IsSuccess.Should().BeTrue();
            r.Value.Should().Be(42);
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

            res.ShouldBeEquivalentTo(Result.Fail<int>("123"));
        }

        [Test]
        public void ReturnsFailWithCustomMessage_FromResultOf_OnException()
        {
            var res = Result.Of<int>(() => { throw new Exception("123"); }, "42");

            res.ShouldBeEquivalentTo(Result.Fail<int>("42"));
        }

        [Test]
        public void ReturnsOk_FromResultOf_WhenNoException()
        {
            var res = Result.Of(() => 42);

            res.ShouldBeEquivalentTo(Result.Ok(42));
        }

        [Test]
        public void UsesNestedResult_WhenUnwrapOk()
        {
            Result.Ok(Result.Ok(42))
                .Unwrap()
                .ShouldBeEquivalentTo(Result.Ok(42));
        }

        [Test]
        public void UsesNestedError_WhenUnwrapFailedFromOk()
        {
            Result.Ok(Result.Fail<int>("msg"))
                .Unwrap()
                .ShouldBeEquivalentTo(Result.Fail<int>("msg"));
        }

        [Test]
        public void UsesTopLevelError_WhenUnwrapFailed()
        {
            Result.Fail<Result<int>>("msg")
                .Unwrap()
                .ShouldBeEquivalentTo(Result.Fail<int>("msg"));
        }

        [Test]
        public void RunOnSuccess_WhenOk()
        {
            var res = Result.Ok(42)
                .OnSuccess(n => n + 10);
            res.ShouldBeEquivalentTo(Result.Ok(52));
        }

        [Test]
        public void SkipOnSuccess_WhenFail()
        {
            var fail = Result.Fail<int>("ошибка");
            var res = fail.OnSuccess(n =>
            {
                Assert.Fail("should not be executed");
                return n;
            });
            res.ShouldBeEquivalentTo(fail);
        }

        [Test]
        public void OnSuccess_ReturnsFail_OnException()
        {
            Func<int, int> continuation = n =>
            {
                throw new Exception("123");
            };
            var res = Result.Ok(42)
                .OnSuccess(continuation);
            res.ShouldBeEquivalentTo(Result.Fail<int>("123"));
        }

        [Test]
        public void RunOnFail_WhenFail()
        {
            var fail = Result.Fail<int>("ошибка");
            var errorHandler = A.Fake<Action<string>>();

            var res = fail.OnFail(errorHandler);

            A.CallTo(() => errorHandler(null)).WithAnyArguments().MustHaveHappened();
            res.ShouldBeEquivalentTo(fail);
        }

        [Test]
        public void SkipOnFail_WhenOk()
        {
            var ok = Result.Ok(42);

            var res = ok.OnFail(v => { Assert.Fail("Should not be called"); });

            res.ShouldBeEquivalentTo(ok);
        }

        [Test]
        public void RunOnSuccess_WhenOk_Scenario()
        {
            var res =
                Result.Ok("1358571172")
                    .OnSuccess(int.Parse)
                    .OnSuccess(i => Convert.ToString(i, 16))
                    .OnSuccess(hex => Guid.Parse(hex + hex + hex + hex));
            res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void RunOnSuccess_WhenOk_ComplexScenario()
        {
            var parsed = Result.Ok("1358571172").OnSuccess(int.Parse);
            var res = parsed
                .OnSuccess(i => Convert.ToString(i, 16))
                .OnSuccess(hex => parsed.Value + " -> " + Guid.Parse(hex + hex + hex + hex));
            res.ShouldBeEquivalentTo(Result.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
        }
    }
}