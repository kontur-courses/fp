using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloudResult
{
    [TestFixture]
    public class Result_Should
    {
        [Test]
        public void Create_Ok()
        {
            var r = ResultIs.Ok(42);
            r.IsSuccess.Should().BeTrue();
            r.GetValueOrThrow().Should().Be(42);
        }

        [Test]
        public void Create_Fail()
        {
            var r = ResultIs.Fail<int>("123");

            r.IsSuccess.Should().BeFalse();
            r.Error.Should().Be("123");
        }

        [Test]
        public void ReturnsFail_FromResultOf_OnException()
        {
            var res = ResultIs.Of<int>(() => { throw new Exception("123"); });

            res.Should().BeEquivalentTo(ResultIs.Fail<int>("123"));
        }

        [Test]
        public void ReturnsFailWithCustomMessage_FromResultOf_OnException()
        {
            var res = ResultIs.Of<int>(() => { throw new Exception("123"); }, "42");

            res.Should().BeEquivalentTo(ResultIs.Fail<int>("42"));
        }

        [Test]
        public void ReturnsOk_FromResultOf_WhenNoException()
        {
            var res = ResultIs.Of(() => 42);

            res.Should().BeEquivalentTo(ResultIs.Ok(42));
        }

        [Test]
        public void RunThen_WhenOk()
        {
            var res = ResultIs.Ok(42)
                .Then(n => n + 10);
            res.Should().BeEquivalentTo(ResultIs.Ok(52));
        }

        [Test]
        public void SkipThen_WhenFail()
        {
            var fail = ResultIs.Fail<int>("������");
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
            Func<int, int> continuation = n => { throw new Exception("123"); };
            var res = ResultIs.Ok(42)
                .Then(continuation);
            res.Should().BeEquivalentTo(ResultIs.Fail<int>("123"));
        }

        [Test]
        public void RunOnFail_WhenFail()
        {
            var fail = ResultIs.Fail<int>("������");
            var errorHandler = A.Fake<Action<string>>();

            var res = fail.OnFail(errorHandler);

            A.CallTo(() => errorHandler(null)).WithAnyArguments().MustHaveHappened();
            res.Should().BeEquivalentTo(fail);
        }

        [Test]
        public void SkipOnFail_WhenOk()
        {
            var ok = ResultIs.Ok(42);

            var res = ok.OnFail(v => { Assert.Fail("Should not be called"); });

            res.Should().BeEquivalentTo(ok);
        }

        [Test]
        public void RunThen_WhenOk_Scenario()
        {
            var res =
                ResultIs.Ok("1358571172")
                    .Then(int.Parse)
                    .Then(i => Convert.ToString(i, 16))
                    .Then(hex => Guid.Parse(hex + hex + hex + hex));
            res.Should().BeEquivalentTo(ResultIs.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void RunThen_WhenOk_ComplexScenario()
        {
            var parsed = ResultIs.Ok("1358571172").Then(int.Parse);
            var res = parsed
                .Then(i => Convert.ToString(i, 16))
                .Then(hex => parsed.GetValueOrThrow() + " -> " + Guid.Parse(hex + hex + hex + hex));
            res.Should().BeEquivalentTo(ResultIs.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
        }

        [Test]
        public void ReplaceError_IfFail()
        {
            Result.Fail("error")
                .ReplaceError(e => "replaced")
                .Should().BeEquivalentTo(Result.Fail("replaced"));
        }

        [Test]
        public void ReplaceError_DoNothing_IfSuccess()
        {
            ResultIs.Ok(42)
                .ReplaceError(e => "replaced")
                .Should().BeEquivalentTo(ResultIs.Ok(42));
        }

        [Test]
        public void ReplaceError_DontReplace_IfCalledBeforeError()
        {
            ResultIs.Ok(42)
                .ReplaceError(e => "replaced")
                .Then(n => ResultIs.Fail<int>("error"))
                .Should().BeEquivalentTo(ResultIs.Fail<int>("error"));
        }

        [Test]
        public void RefineError_AddErrorMessageBeforePreviousErrorText()
        {
            var calculation = Result.Fail("No connection");
            calculation.RefineError("Posting results to db")
                .Should().BeEquivalentTo(Result.Fail("Posting results to db. No connection"));
        }
    }
}
