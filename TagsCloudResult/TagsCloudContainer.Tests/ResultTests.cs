// using System;
// using System.Collections.Generic;
// using FluentAssertions;
// using Moq;
// using NUnit.Framework;
//
// namespace TagsCloudContainer.Tests
// {
//     public class ResultTests
//     {
//         private Exception exception;
//         private Result<int> failedResult;
//         private string value;
//         private Result<string> successResult;
//
//         [SetUp]
//         public void SetUp()
//         {
//             exception = new Exception();
//             failedResult = new Result<int>(exception);
//             value = "Success Result Value";
//             successResult = new Result<string>(value);
//         }
//
//         [Test]
//         public void GetValue_ReturnValue_IfResultSucceed()
//         {
//             successResult.Value
//                 .Should().Be(value);
//         }
//
//         [Test]
//         public void GetException_ReturnException_IfResultNotSucceed()
//         {
//             failedResult.Exception
//                 .Should().Be(exception);
//         }
//
//         [Test]
//         public void GetValue_ThrowsException_IfResultNotSucceed()
//         {
//             Assert.Throws<Exception>(
//                 () =>
//                 {
//                     var _ = failedResult.Value;
//                 });
//         }
//
//         [Test]
//         public void GetException_ThrowsException_IfResultSucceed()
//         {
//             Assert.Throws<Exception>(
//                 () =>
//                 {
//                     var _ = successResult.Exception;
//                 });
//         }
//         //
//         // [Test]
//         // public void OnSuccess_WithSuccessResult_CallFunc()
//         // {
//         //     var nextValue = "next value";
//         //     var nextResult = successResult.OnSuccess(x => nextValue);
//         //
//         //     nextResult.Value.Should().Be(nextValue);
//         // }
//         //
//         // [Test]
//         // public void OnSuccess_WithFailResult_DontCallFunc()
//         // {
//         //     var funcMock = new Mock<Func<int, int>>();
//         //
//         //     failedResult.OnSuccess(funcMock.Object);
//         //
//         //     funcMock.Verify(m => m(It.IsAny<int>()), Times.Never);
//         // }
//         //
//         // [Test]
//         // public void OnSuccess_WithFailResult_ReturnInnerException()
//         // {
//         //     var nextResult = failedResult.OnSuccess(_ => _);
//         //
//         //     nextResult.Exception.Should().Be(exception);
//         // }
//
//         [Test]
//         public void Zip_WithSuccessResults_ReturnSuccessResult()
//         {
//             var secondSuccessResult = new Result<string>("A");
//
//             var resultZip = Result.Zip(successResult, secondSuccessResult);
//
//             resultZip.Value.Should().Be((value, "A"));
//         }
//
//         [Test]
//         public void Zip_WithBothFailedResults_ReturnFirstResultException()
//         {
//             var secondFailedResult = new Result<int>(new Exception());
//
//             var resultZip = Result.Zip(failedResult, secondFailedResult);
//
//             resultZip.Exception.Should().Be(exception);
//         }
//
//         [Test]
//         public void Zip_WithSecondFailedResults_ReturnSecondResultException()
//         {
//             var resultZip = Result.Zip(successResult, failedResult);
//
//             resultZip.Exception.Should().Be(exception);
//         }
//     }
// }