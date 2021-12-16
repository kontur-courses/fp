using FluentAssertions;
using Moq;
using NUnit.Framework;
using TagsCloudApp.WordsLoading;

namespace TagsCloud.Tests
{
    public class TextFileLoaderTests
    {
        private TextFileLoader loader;

        [SetUp]
        public void SetUp()
        {
            loader = new Mock<TextFileLoader>().Object;
        }

        [Test]
        public void LoadText_WithNotExistingFile_ReturnFailResult()
        {
            loader.LoadText("sef")
                .IsSuccess.Should().BeFalse();
        }
    }
}