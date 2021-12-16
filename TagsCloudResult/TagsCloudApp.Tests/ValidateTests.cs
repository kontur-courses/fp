using FluentAssertions;
using NUnit.Framework;
using TagsCloudApp.Actions;
using TagsCloudContainer.Results;

namespace TagsCloud.Tests
{
    public class ValidateTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void Positive_WhenValueNegative_ReturnFailResult(int value)
        {
            Validate.Positive("", value)
                .IsSuccess.Should().BeFalse();
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(7)]
        public void Positive_WhenValuePositive_ReturnOkResult(int value)
        {
            Validate.Positive("", value)
                .Should().BeEquivalentTo(Result.Ok(value));
        }
    }
}