using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer.Tests
{
    public class ResultLinqExtensionsTests
    {
        private IEnumerable<int> values;
        private IEnumerable<Result<int>> successResults;

        [SetUp]
        public void SetUp()
        {
            values = values = Enumerable.Range(0, 10).ToList();
            successResults = values.Select(Result.Ok);
        }

        [Test]
        public void CombineResults_WhenAllResultsSuccess_ReturnValuesEnumerable()
        {
            var finaleResult = successResults.CombineResults();

            finaleResult.GetValueOrThrow().Should().BeEquivalentTo(values);
        }

        [Test]
        public void CombineResults_WithFailedResults_ReturnFirstFailedException()
        {
            var failedResults = Enumerable.Range(0, 10).Select(i => new Result<int>(i.ToString()));
            var allResults = successResults.Concat(failedResults);

            var finaleResult = allResults.CombineResults();

            finaleResult.Error.Should().BeEquivalentTo("0");
        }
    }
}