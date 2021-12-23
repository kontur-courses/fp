using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace ResultMonad.Tests
{
    [TestFixture]
    public class ResultQueryExpressionTests
    {
        [Test]
        public void SelectMany_ShouldSupportLinqMethodChaining()
        {
            var parseResult =
                "1358571172".ParseIntResult()
                    .SelectMany(i => Convert.ToString(i, 16).AsResult())
                    .SelectMany(hex => (hex + hex + hex + hex).ParseGuidResult());
            parseResult.Should().BeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SelectMany_ShouldSupportLinqMethodChaining_WithResultSelector()
        {
            var parseResult =
                "1358571172".ParseIntResult()
                    .SelectMany(i => Convert.ToString(i, 16).AsResult(), (intResult, hex) => new { i = intResult, hex })
                    .SelectMany(t => (t.hex + t.hex + t.hex + t.hex).ParseGuidResult());
            parseResult.Should().BeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SelectMany_ShouldSupportQueryExpressions()
        {
            var parseResult =
                from intResult in "1358571172".ParseIntResult()
                from hex in Convert.ToString(intResult, 16).AsResult()
                from guid in (hex + hex + hex + hex).ParseGuidResult()
                select guid;
            parseResult.Should().BeOfType<Result<Guid>>();
            parseResult.Should().BeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SelectMany_ShouldSupportQueryExpressions_WithComplexSelect()
        {
            var parseResult =
                from intResult in "1358571172".ParseIntResult()
                from hex in Convert.ToString(intResult, 16).AsResult()
                from guid in (hex + hex + hex + hex).ParseGuidResult()
                select intResult + " -> " + guid;
            parseResult.Should().BeOfType<Result<string>>();
            parseResult.Should().BeEquivalentTo(Result.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
        }

        [Test]
        public void SelectMany_ShouldReturnFail_FromSelectMany_WhenErrorAtTheEnd()
        {
            var parseResult =
                from intResult in "0".ParseIntResult()
                from hex in Convert.ToString(intResult, 16).AsResult()
                from guid in (hex + hex + hex + hex).ParseGuidResult("error is here")
                select guid;
            parseResult.Should().BeEquivalentTo(Result.Fail<Guid>("error is here"));
        }

        [Test]
        public void SelectMany_ShouldReturnFail_FromSelectMany_WhenExceptionOnSomeStage()
        {
            var parseResult =
                from intResult in "1358571172".ParseIntResult()
                from hex in Result.Of(() => Convert.ToString(intResult, 100500), "error is here")
                from guid in (hex + hex + hex + hex).ParseGuidResult()
                select guid;
            parseResult.Should().BeEquivalentTo(Result.Fail<Guid>("error is here"));
        }

        [Test]
        public void SelectMany_ShouldReturnFail_FromSelectMany_WhenErrorAtTheBeginning()
        {
            var parseResult =
                from intResult in "UNPARSABLE".ParseIntResult("error is here")
                from hex in Convert.ToString(intResult, 16).AsResult()
                from guid in (hex + hex + hex + hex).ParseGuidResult()
                select guid;
            parseResult.Should().BeEquivalentTo(Result.Fail<Guid>("error is here"));
        }
    }

    [TestFixture]
    public class ResultEnumerableExtensionsTests
    {
        [Test]
        public void Traverse_ShouldHasValue_WhenCollectionWithNoError()
        {
            var numbers = new[] { 1, 2, 3 };
            var results = numbers.Select(Result.Ok);

            var traversed = results.Traverse();

            traversed.GetValueOrThrow().Should().ContainInOrder(numbers);
        }

        [Test]
        public void Traverse_ShouldHasError_WhenMiddleElementWithError()
        {
            var fail = Result.Fail<int>("Error");
            var results = new[]
            {
                1.AsResult(),
                fail,
                2.AsResult()
            };

            var traversed = results.Traverse();

            traversed.IsSuccess.Should().BeFalse();
            traversed.Error.Should().NotBeNull();
        }

        [Test]
        public void Traverse_ShouldHasOnlyFirstError_WhenMoreThanOneErrorInSource()
        {
            var fail = Result.Fail<int>("Error");
            var otherFail = Result.Fail<int>("Error2");
            var results = new[]
            {
                fail,
                otherFail
            };

            var traversed = results.Traverse();

            traversed.Error.Should().Be(fail.Error);
        }

        [Test]
        public void Traverse_ShouldNotEnumerate_WhenFoundError()
        {
            var fail = Result.Fail<int>("Error");
            var count = 0;
            var results = new[] { fail, 1, 2 }.Select(element =>
            {
                count++;
                return element;
            });

            results.Traverse();

            count.Should().Be(1);
        }
    }
}