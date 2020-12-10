using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.ConvertersAndCheckersForSettings.CheckerForFile;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class FileExistsСhecker_Should
    {
        private FileExistsСhecker _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new FileExistsСhecker();
        }

        [Test]
        public void GetProvenPath_IsSuccess_WhenExistFile()
        {
            var act = _sut.GetProvenPath(Path.Combine(Directory.GetCurrentDirectory(), "mystem.exe"));

            act.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetProvenPath_IsSuccess_WhenFileInCurrentDirectory()
        {
            var act = _sut.GetProvenPath("mystem.exe");

            act.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetProvenPath_IsNotSuccess_WhenNotExistFile()
        {
            var act = _sut.GetProvenPath($@"directory{Path.DirectorySeparatorChar}random file.txt");

            act.IsSuccess.Should().BeFalse();
        }
    }
}