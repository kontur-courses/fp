using System;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests.Tests
{
    [TestFixture]
    public class ResultTests
    {
        private readonly TestObject testObject;

        public ResultTests()
        {
            testObject = new TestObject();
        }
        
        [Test]
        public void AsResult_WithTestObject_ShouldReturnResultOfObject()
        {
            var objAsResult = testObject.AsResult();
            
            Assert.AreEqual(typeof(Result<TestObject>), objAsResult.GetType());
        }
        
        [Test]
        public void Ok_WithTestObject_ShouldReturnResultWithNullError()
        {
            var result = Result.Ok(testObject);
            
            Assert.AreEqual(null, result.Error);
        }
        
        [Test]
        public void Fail_WithTestObject_ShouldReturnResultWithError()
        {
            const string errorMessage = "fail";
            
            var result = Result.Fail<TestObject>(errorMessage);
            
            Assert.AreEqual(errorMessage, result.Error);
        }
        
        [Test]
        public void Of_WithIncorrectFunc_ShouldReturnFailResult()
        {
            var result = Result.Of<TestObject>(() => throw new Exception());

            Assert.AreEqual(false, result.IsSuccess);
        }
        
        [Test]
        public void Of_WithCorrectFunc_ShouldReturnOkResult()
        {
            var result = Result.Of(() => new TestObject());

            Assert.AreEqual(true, result.IsSuccess);
        }
        
        [Test]
        public void Then_WithIncorrectFunc_ShouldReturnFailResult()
        {
            var result = testObject
                .AsResult()
                .Then(x => throw new Exception());

            Assert.AreEqual(false, result.IsSuccess);
        }
        
        [Test]
        public void Then_WithCorrectFunc_ShouldReturnOkResult()
        {
            var result = testObject
                .AsResult()
                .Then(x => new TestObject());

            Assert.AreEqual(true, result.IsSuccess);
        }
    }
}
