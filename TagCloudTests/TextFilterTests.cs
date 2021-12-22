using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.TextHandlers.Filters;

namespace TagCloudTests
{
    [TestFixture]
    public class TextFilterTests
    {
        [Test]
        public void Filter_ShouldFilterWords_IfUsingOtherFilterClass()
        {
            var words = new[] { "12345", "123", "12", "1", "3334" };
            var expected = new[] { "12345", "3334" }.AsResult();
            var sut = new TextFilter(new BoringWordsFilter());

            var filtered = sut.Filter(words);

            filtered.Should().BeEquivalentTo(expected);
        }
    }
}