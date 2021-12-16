using System;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Results;

namespace TagsCloudContainer.Tests.FluentAssertionsExtensions
{
    public class FluentAssertionsExtensionsTests
    {
        [Test]
        public void BeFailed_AssertionTrue_WithMatchingErrors()
        {
            Result.Fail("err")
                .Should().BeFailed("err");
        }

        [Test]
        public void BeFailed_WithoutError_AssertOnlySuccess()
        {
            Result.Fail("err")
                .Should().BeFailed();
        }

        [Test]
        public void BeFailed_AssertionFalse_WithDifferentErrors()
        {
            AssertionShouldFail(() =>
            {
                Result.Fail("one")
                    .Should().BeFailed("two");
            });
        }

        [Test]
        public void BeOk_AssertionTrue_WithSuccessResult()
        {
            Result.Ok()
                .Should().BeOk();
        }

        [Test]
        public void BeOk_WithoutValue_AssertOnlySuccess()
        {
            Result.Ok("sef")
                .Should().BeOk();
        }

        [Test]
        public void BeOk_AssertionFalse_WithFailedResult()
        {
            AssertionShouldFail(() =>
            {
                Result.Fail("err")
                    .Should().BeOk();
            });
        }

        private static void AssertionShouldFail(Action test)
        {
            test.Should().ThrowExactly<AssertionException>();
        }
    }
}