using FluentAssertions;

namespace TagCloud.Tests
{
    [TestFixture]
    public class FrequencyDictionaryBuilderTests
    {
        private FrequencyDictionaryBuilder<string> builder;

        [SetUp]
        public void Setup()
        {
            builder = new FrequencyDictionaryBuilder<string>();
        }

        [Test]
        public void Build_PassValidArray_BuildsCorrectDict()
        {
            var words = new[] { "Машина", "Самолет", "Велосипед", "Велосипед" };
            var correctDict = new Dictionary<string, int>() 
            {
                ["Машина"] = 1,
                ["Самолет"] = 1,
                ["Велосипед"] = 2
            };

            var dict = builder.Build(words);

            dict.Should().BeEquivalentTo(correctDict);
        }
    }
}
