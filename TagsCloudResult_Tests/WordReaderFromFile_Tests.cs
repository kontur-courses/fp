using NUnit.Framework;
using System;
using System.Linq;
using FluentAssertions;
using TagsCloudResult;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult_Tests
{
    [TestFixture]
    public class WordReaderFromFileTests
    {
        private static readonly AppSettings settings = AppSettingsForTests.Settings;

        [SetUp]
        public void SetUp()
        {
            System.IO.File.WriteAllText(settings.Path, "Some\ntest\ntext\n");
        }

        [Test]
        public void ShouldReturnEnumerationOfWord_IfGetCorrectFile()
        {
            var actual = WordReaderFromFile.ReadWords(settings);

            actual.IsSuccess.Should().BeTrue();
            actual.GetValueOrThrow().Count().Should().Be(3);
        }

        [Test]
        public void ShouldReturnCorrectWordsWithoutAnySpecialSymbol()
        {
            var actual = WordReaderFromFile.ReadWords(settings);

            actual.IsSuccess.Should().BeTrue();
            actual.GetValueOrThrow().Should().Contain(new[] {"Some", "test", "text"});
        }

        [Test]
        public void ShouldReturnErrorMessageNoSuchFile_IfGetNotExistedFile()
        {
            var badPathSetting = new AppSettings(default(ImageSetting),
                default(WordSetting),
                default(AlgorithmsSettings)
                , "This file can`t exist in this world");
            var actual = WordReaderFromFile.ReadWords(badPathSetting);

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Should().Be($"No such file: {badPathSetting.Path}");
        }

        [Test]
        public void ShouldReturnErrorMessageFileOccupied_IfUnableToLoadFile()
        {
            var fileStream = System.IO.File.OpenText(settings.Path);
            var actual = WordReaderFromFile.ReadWords(settings);
            fileStream.Close();

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Should().Be($"File can`t be open, try to free him: {settings.Path}, from other resources");
        }

        [TearDown]
        public void TearDown()
        {
            System.IO.File.Delete(settings.Path);
        }
    }
}