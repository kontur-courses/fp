using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests

{
    public class PictureBoxImageHolder_Should
    {
        [Test]
        public void PictureBoxImageHolder_ShouldThrowException_WhenNotRecreateImage()
        {
            var actual = new PictureBoxImageHolder().GetImageSize();

            actual.Error.Should().Be("Call PictureBoxImageHolder.RecreateImage before other method call!");
        }
    }
}
