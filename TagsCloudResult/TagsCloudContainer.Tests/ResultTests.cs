using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TagsCloudContainer.Results;
using TagsCloudContainer.Tests.FluentAssertionsExtensions;

namespace TagsCloudContainer.Tests
{
    public class ResultTests
    {
        [Test]
        public void GetValueOrThrow_WithOkResult_ReturnValue()
        {
            Result.Ok(42)
                .GetValueOrThrow().Should().Be(42);
        }

        [Test]
        public void GetValueOrThrow_WithFailResult_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Result.Fail<int>("123").GetValueOrThrow());
        }

        [Test]
        public void ResultOk_WithoutValue_CreatesSuccessResult()
        {
            Result.Ok()
                .IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ResultOk_WithValue_CreatesSuccessResult()
        {
            Result.Ok("se")
                .IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ResultFail_CreatesNotSuccessResult()
        {
            Result.Fail<string>("se")
                .IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ResultOf_ThrowingExceptionFunc_ReturnsFail()
        {
            Result.Of<int>(() => throw new Exception("123"))
                .Should().BeFailed("123");
        }

        [Test]
        public void ResultOf_ThrowingExceptionFuncWithCustomError_ReturnsFailWithError()
        {
            Result.Of<int>(() => throw new Exception("123"), "new error")
                .Should().BeFailed("new error");
        }

        [Test]
        public void ResultOf_SuccessFunc_ReturnsFuncValue()
        {
            Result.Of(() => 3)
                .GetValueOrThrow().Should().Be(3);
        }

        [Test]
        public void ResultOfAction_ThrowingException_ReturnsFail()
        {
            Result.OfAction(() => throw new Exception("123"))
                .Should().BeFailed("123");
        }

        [Test]
        public void ResultOfAction_ThrowingExceptionWithCustomError_ReturnsFailWithError()
        {
            Result.OfAction(() => throw new Exception("123"), "new error")
                .Should().BeFailed("new error");
        }

        [Test]
        public void ResultOfAction_WithSuccessAction_CallAction()
        {
            var action = new Mock<Action>();
            Result.OfAction(action.Object);
            action.Verify(a => a(), Times.Once);
        }

        [Test]
        public void AsResult_CreateSuccessResult()
        {
            5.AsResult()
                .Should().BeOk(5);
        }

        [Test]
        public void Then_CallNexFunc_WhenOk()
        {
            var result = Result.Ok(41)
                .Then(n => n + 5);

            result.Should().BeOk(46);
        }

        [Test]
        public void Then_DontCallNexFunc_WhenFail()
        {
            var next = new Mock<Func<int, int>>();
            var result = Result.Fail<int>("err")
                .Then(n => next.Object(n));

            result.Should().BeFailed("err");
            next.Verify(f => f(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void Then_WhenContinuationThrow_ReturnFail()
        {
            Func<int, int> next = n => throw new Exception("err");

            var result = Result.Ok(22)
                .Then(next);

            result.Should().BeFailed("err");
        }

        [Test]
        public void Then_WhenContinuationReturnOk_CallNext()
        {
            var result = Result.Ok(22)
                .Then(n => Result.Ok(n + 10))
                .Then(n => n + 10);

            result.Should().BeOk(42);
        }

        [Test]
        public void Then_WhenContinuationReturnFail_DontCallNext()
        {
            var next = new Mock<Func<int, int>>();
            var result = Result.Ok(22)
                .Then(n => Result.Fail<int>("err"))
                .Then(n => next.Object(n));

            result.Should().BeFailed("err");
            next.Verify(f => f(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void Then_CallNext_WhenOk()
        {
            var next = new Mock<Func<Result<None>>>();
            Result.Ok()
                .Then(next.Object);

            next.Verify(a => a(), Times.Once);
        }

        [Test]
        public void Then_DontCallNext_WhenFail()
        {
            var next = new Mock<Func<Result<None>>>();
            Result.Fail("err")
                .Then(next.Object);

            next.Verify(a => a(), Times.Never);
        }

        [Test]
        public void Then_ChainFunc_Scenario()
        {
            var list = new List<double>();

            var result = Result.Ok("1")
                .Then(double.Parse)
                .Then(Math.Acos)
                .Then(list.Add);

            result.Should().BeOk();
            list.Should().BeEquivalentTo(new List<double> {0});
        }

        [Test]
        public void OnFail_WithOkResult_DoNothing()
        {
            Result.Ok()
                .OnFail(e => { Assert.Fail(); });
        }

        [Test]
        public void OnFail_WithFailedResult_CallFunc()
        {
            var onFail = new Mock<Action<string>>();

            Result.Fail("err")
                .OnFail(e => onFail.Object(e));

            onFail.Verify(f => f("err"), Times.Once);
        }

        [Test]
        public void ReplaceError_WithOkResult_DoNothing()
        {
            var replacer = new Mock<Func<string, string>>();
            Result.Ok()
                .ReplaceError(replacer.Object);

            replacer.Verify(f => f(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ReplaceError_WithFailResult_ReplaceError()
        {
            var result = Result.Fail("err")
                .ReplaceError(err => "new err");

            result.Should().BeFailed("new err");
        }

        [Test]
        public void ReplaceError_IfCalledBeforeError_DoNothing()
        {
            var result = Result.Ok()
                .ReplaceError(e => "new err")
                .Then(() => Result.Fail("err"));

            result.Should().BeFailed("err");
        }

        [Test]
        public void RefineError_AddErrorMessageBeforePrevious()
        {
            var calculation = Result.Fail<None>("No connection.");
            var result = calculation
                .RefineError("Posting results to db.");

            result.Should().BeFailed("Posting results to db. No connection.");
        }

        [Test]
        public void ResultImplicitlyConvertValues_ToOkResult()
        {
            Result<int> result = 5;

            result.Should().BeOk(5);
        }
    }
}