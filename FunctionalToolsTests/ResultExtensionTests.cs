using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using FunctionalTools;
using NUnit.Framework;

namespace FunctionalToolsTests
{
    [TestFixture]
    internal class ResultExtensionTests
    {
        [TestCase(42, Description = "primitive type")]
        [TestCase("42", Description = "reference type")]
        public void AsResult_ShouldReturnSuccessResult<T>(T obj)
        {
            var result = obj.AsResult();

            result.IsSuccess.Should().BeTrue();
        }

        [TestCase(42, Description = "primitive type")]
        [TestCase("42", Description = "reference type")]
        public void AsResult_ShouldReturnResultWithThisValue<T>(T obj)
        {
            var result = obj.AsResult();

            result.GetValueOrThrow().Should().BeEquivalentTo(obj);
        }

        [Test]
        public void
            Then_PreviousIsSuccessAndParameterReturnValueOfTypeResult_ShouldReturnOutputValueOfParameterExecution()
        {
            var data = 42;
            Result<int> Continuation(int a) => data.AsResult();

            var actual = Result.Ok(40).Then(Continuation);

            actual.Should().BeEquivalentTo(Continuation(0));
        }


        [Test]
        public void Then_PreviousResultIsFailedAndParameterReturnValueOfTypeResult_ShouldReturnFailedResult()
        {
            var data = 42;
            Result<string> Continuation(int a) => data.ToString().AsResult();
            var expected = Result.Fail<string>("This is error");

            var actual = Result.Fail<int>("This is error").Then(Continuation);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Then_InputResultIsSuccess_ShouldReturnOutputValueOfParameterExecutionAsResult()
        {
            var data = 42;
            int Continuation(int a) => data;

            var actual = Result.Ok(40).Then(Continuation);

            actual.Should().BeEquivalentTo(Continuation(0).AsResult());
        }

        [Test]
        public void Then_PreviousResultIsFailed_ShouldReturnFailedResult()
        {
            var data = 42;
            Result<string> Continuation(int a) => data.ToString();
            var expected = Result.Fail<string>("This is error");

            var actual = Result.Fail<int>("This is error").Then(Continuation);

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void OnFail_PreviousIsFail_ShouldExecuteHandleError()
        {
            var error = "This is error";
            var handleError = A.Fake<Action<string>>();

            Result.Fail<string>(error).OnFail(handleError);

            A.CallTo(() => handleError(error)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void OnFail_PreviousIsSuccess_ShouldNotExecuteHandleError()
        {
            var handleError = A.Fake<Action<string>>();

            Result.Ok().OnFail(handleError);

            A.CallTo(() => handleError(null))
                .WithAnyArguments()
                .MustNotHaveHappened();
        }

        [Test]
        public void ReplaceError_PreviousIsFailed_ShouldReplaceError()
        {
            string Adder(string a) => $"{a}: was changed";

            var actual = Result.Fail<int>("error").ReplaceError(Adder);

            actual.Error.Should().BeEquivalentTo("error: was changed");
        }

        [Test]
        public void ReplaceError_PreviousIsSuccess_ShouldReturnPreviousResult()
        {
            var previous = Result.Ok(42);
            string Adder(string a) => $"{a}: was changed";

            var actual = previous.ReplaceError(Adder);

            actual.Should().BeEquivalentTo(previous);
        }

        [Test]
        public void RefineError_PreviousIsFailed_ShouldRefineError()
        {
            var actual = Result.Fail<int>("error").RefineError("added error");

            actual.Error.Should().BeEquivalentTo("added error. error");
        }

        [Test]
        public void RefineError_PreviousIsSuccess_ShouldReturnPreviousResult()
        {
            var previous = Result.Ok(42);

            var actual = previous.RefineError("added error");

            actual.Should().BeEquivalentTo(previous);
        }
    }
}