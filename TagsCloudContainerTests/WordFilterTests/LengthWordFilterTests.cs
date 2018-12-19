using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.Settings;
using TagsCloudContainer.WordFilter;

namespace TagsCloudContainerTests.WordFilterTests
{
    [TestFixture]
    public class LengthWordFilterTests
    {
        [Test]
        public void LengthFilter_ShouldSkipShortWords()
        {
            var option = new Option();
            option.SmallestLength = 2;
            var filterSettings = new FilterSettings(option);
            var text = new[] {"hi", "verylong", "medium"};
            var filter = new LengthWordFilter(filterSettings);
            var expectedResult = new[] {"verylong", "medium"};

            var result = text.Where(word => filter.Validate(word));

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}