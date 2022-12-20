using System;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud;

namespace TagsCloudTests
{
    [TestFixture]
    public class ResultHandlerShould
    {
        private ResultHandler<int> handler;

        [SetUp]
        public void Setup()
        {
            handler = new ResultHandler<int>(42);
        }

        [Test]
        public void Create_Ok()
        {
            handler.IsSuccess.Should().BeTrue();
            handler.Value.Should().Be(42);
        }

        [Test]
        public void Create_Fail()
        {
            var r = handler.Fail("123");

            r.IsSuccess.Should().BeFalse();
            r.Error.Should().Be("123");
        }

        [Test]
        public void RunThenDoWorkWithValue_WhenOk()
        {
            var res = handler.ThenDoWorkWithValue(x => x + 10);

            res.Should().BeEquivalentTo(new ResultHandler<int>(52));
        }

        [Test]
        public void RunThen_WhenContinuationIsOk()
        {
            var res = handler.ThenDoWorkWithValue(n => n + 10);
            var expected = new ResultHandler<int>(52);
            res.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void SkipThen_WhenFail()
        {
            var fail = handler.Fail("������");
            var called = false;
            fail.Then(n =>
            {
                called = true;
                return n;
            });
            called.Should().BeFalse();
        }

        [Test]
        public void RunThen_WhenOk_Scenario()
        {
            var res = new ResultHandler<string>("1358571172")
                .ThenDoWorkWithValue(x => int.Parse(x))
                .ThenDoWorkWithValue(i => Convert.ToString(i, 16))
                .ThenDoWorkWithValue(hex => Guid.Parse(hex + hex + hex + hex));
            res.Should().BeEquivalentTo(new ResultHandler<Guid>(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void RunThen_WhenOk_ComplexScenario()
        {
            var parsed = new ResultHandler<string>("1358571172").ThenDoWorkWithValue(x => int.Parse(x));
            var res = parsed
                .ThenDoWorkWithValue(i => Convert.ToString(i, 16))
                .ThenDoWorkWithValue(hex => parsed.Value + " -> " + Guid.Parse(hex + hex + hex + hex));
            res.Should()
                .BeEquivalentTo(new ResultHandler<string>("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
        }

        [Test]
        public void ReplaceError_IfFail()
        {
            var res = handler.Fail("error")
                .ReplaceError(e => "replaced");

            res.Invoking(x => x.Value)
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("replaced");
        }

        [Test]
        public void ReplaceError_DoNothing_IfSuccess()
        {
            handler.ReplaceError(e => "replaced")
                .Should().BeEquivalentTo(new ResultHandler<int>(42));
        }

        [Test]
        public void ReplaceError_DontReplace_IfCalledBeforeError()
        {
            var res = handler.ReplaceError(e => "replaced")
                .Then(n => n.Fail("error"));

            res.Invoking(x => x.Value)
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("error");
        }

        [Test]
        public void RefineError_AddErrorMessageBeforePreviousErrorText()
        {
            var calculation = handler.Fail("No connection");
            var res = calculation.RefineError("Posting results to db");

            res.Invoking(x => x.Value)
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Posting results to db. No connection");
        }
    }
}