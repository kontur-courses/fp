using TagsCloudContainer.Interfaces;
using TagsCloudContainer.TagsCloud;

namespace TagsCloudContainerTests
{
    [TestFixture]
    public class TagCloudGeneratorTests
    {
        private ITagCloudGenerator sut;

        public TagCloudGeneratorTests()
        {
            sut = new TagCloudGenerator();
        }

        [Test]
        public void GenerateTagCloud_ValidData_ReturnsImage()
        {
            var words = new[] { "apple", "banana", "orange", "apple", "banana" };
            var imageSettings = new ImageSettings();

            var tagCloudImage = sut.GenerateTagCloud(words, imageSettings);

            Assert.IsNotNull(tagCloudImage);
        }
    }
}
