using System;
using FluentAssertions;
using FunctionalTools;
using NUnit.Framework;

namespace FunctionalToolsTests
{
    [TestFixture]
    internal class ResultTests
    {
        private const string ErrorMessage = "This is error";

        [Test]
        public void Ok_ShouldReturnSuccessResult()
        {
            var result = Result.Ok();

            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetValueOrThrow_ResultIsSuccess_ShouldReturnValue()
        {
            var data = 42;
            var result = Result.Ok(data);

            result.GetValueOrThrow().Should().Be(data);
        }

        [Test]
        public void GetValueOrThrow_ResultIsFail_ShouldThrowException()
        {
            var result = Result.Fail<int>(ErrorMessage);

            Action act = () => result.GetValueOrThrow();

            act.Should().Throw<Exception>(ErrorMessage);
        }

        [Test]
        public void Fail_ShouldReturnFailedResult()
        {
            var result = Result.Fail<int>("This is error");

            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void Fail_ShouldReturnResultWithGivenError()
        {
            var result = Result.Fail<int>(ErrorMessage);

            result.Error.Should().BeEquivalentTo(ErrorMessage);
        }

        [Test]
        public void Of_FactoryNotThrowException_ShouldReturnSuccessResult()
        {
            var result = Result.Of(() => 42);

            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void Of_FactoryReturnValue_ShouldReturnSuccessResultWithThisValue()
        {
            var data = 42;
            var result = Result.Of(() => data);

            result.GetValueOrThrow().Should().Be(data);
        }

        [Test]
        public void Of_FactoryThrowException_ShouldReturnFailedResult()
        {
            double Factory() => throw new Exception();

            var result = Result.Of(Factory);

            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void Of_FactoryThrowExceptionWithMessage_ShouldReturnResultWithThisMessage()
        {
            double Factory() => throw new Exception(ErrorMessage);

            var result = Result.Of(Factory);

            result.Error.Should().BeEquivalentTo(ErrorMessage);
        }

        [Test]
        public void OfAction_ThrowException_ShouldReturnFailedResult()
        {
            void Act() => throw new ArgumentException();

            var result = Result.OfAction(Act);

            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void OfAction_ThrowExceptionWithMessage_ShouldReturnResultWithThisMessage()
        {
            void Act() => throw new ArgumentException(ErrorMessage);

            var result = Result.OfAction(Act);

            result.Error.Should().BeEquivalentTo(ErrorMessage);
        }

        [Test]
        public void OfAction_NotTrowException_ShouldReturnSuccessResult()
        {
            void Act()
            {
            }

            var result = Result.OfAction(Act);
            result.IsSuccess.Should().BeTrue();
        }
    }
}