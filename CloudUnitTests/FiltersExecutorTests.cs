using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using TagCloud.WordFilter;

namespace TagCloudUnitTests
{
    [TestFixture]
    internal class FiltersExecutorTests
    {
        [Test]
        public void Conver_ReturnsLowerCaseWords_WhenToLowerConverterRegistred()
        {
            var filtersExecutor = new FiltersExecutor(new IWordFilter[] { new BoringWordFilter() });

            var inputWords = new List<string>() { "one", "two", "three", "four", "five", "six" };

            var expectedWords = new List<string>() {"three", "four", "five"};

            var actualWords = filtersExecutor.Filter(inputWords);

            actualWords.IsSuccess.Should().BeTrue();
            actualWords.GetValueOrThrow().Should().BeEquivalentTo(expectedWords);
        }
    }
}
