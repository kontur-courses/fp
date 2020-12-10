using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.ConvertersAndCheckersForSettings.CheckerForDirectory;

namespace TagsCloudTests.UnitTests.ConvertersAndCheckersForSettings_Tests
{
    public class DirectoryExistsChecker_Should
    {
        private DirectoryExistsChecker _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new DirectoryExistsChecker();
        }

        [Test]
        public void GetProvenDirectory_IsSuccess_WhenExistDirectory()
        {
            var act = _sut.GetExistingDirectory(Directory.GetCurrentDirectory());

            act.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetProvenDirectory_IsSuccess_WhenCurrentDirectory()
        {
            var act = _sut.GetExistingDirectory("file name");

            act.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void GetProvenDirectory_IsNotSuccess_WhenNotExistDirectory()
        {
            var act = _sut.GetExistingDirectory($@"directory{Path.DirectorySeparatorChar}random directory");

            act.IsSuccess.Should().BeFalse();
        }
    }
}