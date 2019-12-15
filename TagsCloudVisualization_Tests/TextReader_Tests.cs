using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.TextReaders;

namespace TagsCloudVisualization_Tests
{
    public class TextReader_Tests
    {
        [Test]
        public void TxtReaderGetText_FileNotFound_ShouldThrowArgumentException()
        {
            var textReaderResult = new TxtReader().ReadText("nonexistentName", Encoding.Default);
            textReaderResult.IsSuccess.Should().BeFalse();
        }
    }
}