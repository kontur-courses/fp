using FluentAssertions;
using TagsCloudContainer.Core.WordsParser;

namespace TagsCloudContainer.Core.Tests
{
    [TestFixture]
    public class FileReaderTests
    {
        [Test]
        public void ReadWords_WhenUnsupportedExt_ShoudBeErrorMessage()
        {
            var reader = new FileReader("path.pdf");
            var result = reader.ReadWords();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Unsupported file extension!");
        }

        [Test]
        public void ReadWords_WhenNoExt_ShoudBeErrorMessage()
        {
            var reader = new FileReader("path");
            var result = reader.ReadWords();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Unsupported file extension!");
        }

        [Test]
        public void ReadWordsShouldntBeSuccess_WhenFileNotExist()
        {
            var reader = new FileReader("dir/dir/input.txt");
            var result = reader.ReadWords();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("File dir/dir/input.txt not found.");
        }

    }
}
