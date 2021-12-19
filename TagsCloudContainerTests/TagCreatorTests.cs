using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Infrastructure.Tags;

namespace TagsCloudContainerTests
{
    internal class TagCreatorTests
    {
        private string[] words;
        private ITagCreator tagCreator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            words = new[] { "music", "guitar", "string", "music" };
        }

        [SetUp]
        public void SetUp()
        {
            tagCreator = new TagCreator();
        }

        [Test]
        public void Should_Throw_OnNoTags()
        {
            tagCreator.CreateTags(new string[] { }).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void Should_ComposeTagsCorrectly()
        {
            var result = tagCreator.CreateTags(words);
            var expected = new[] {
                new Tag(0.5, "music"),
                new Tag(0.25, "guitar"),
                new Tag(0.25, "string")
            };

            result.Value.Should().BeEquivalentTo(expected);
        }
    }
}
