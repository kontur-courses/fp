using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.App.Layouter;

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
