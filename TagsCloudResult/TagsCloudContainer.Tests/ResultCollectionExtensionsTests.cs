using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TagsCloudContainer.Results;
using TagsCloudContainer.Tests.FluentAssertionsExtensions;

namespace TagsCloudContainer.Tests
{
    public class ResultCollectionExtensionsTests
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

            finaleResult.Should().BeOk(values);
        }

        [Test]
        public void CombineResults_WithFailedResults_ReturnFirstFailedException()
        {
            var failedResults = Enumerable.Range(0, 10).Select(i => Result.Fail<int>(i.ToString()));
            var allResults = successResults.Concat(failedResults);

            var finaleResult = allResults.CombineResults();

            finaleResult.Should().BeFailed("0");
        }
    }
}