using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudGenerator.WordsHandler.Filters;

namespace TagsCloudGeneratorTests.WordsHandler.FiltersTests
{
    public class PredicateFilterTests
    {
        [Test]
        public void Filter_ArgumentIsNull_ShouldReturnResultWithError()
        {
            Dictionary<string, int> data = null;
            var filter = new PredicateFilter(x=> x.Value<5);

            filter.Filter(data).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void Filter_ShouldApplyPredicateForAllElements()
        {
            var data = new Dictionary<string, int>
            {
                ["fish"] = 2,
                ["sun"] = 1,
                ["cat"] = 40,
                ["sofa"] = 22,
                ["garden"] = 4
            };
            var expected = new Dictionary<string, int>
            {
                ["fish"] = 2,
                ["sun"] = 1,
                ["garden"] = 4
            };
            var filter = new PredicateFilter(x => x.Value > 5);

            var actual = filter.Filter(data).GetValueOrThrow();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}