using System.Text.RegularExpressions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Core.WordsParsing.WordsReading;

namespace TagCloud.Tests.TextReaders
{
    [TestFixture]
    public class GeneralWordsReader_Should : AbstractWordsReader_Should<GeneralWordsReader>
    {
        private GeneralWordsReader generalWordsReader;
        private IWordsReader ext1WordsReader;
        private IWordsReader ext2WordsReader;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ext1WordsReader = A.Fake<IWordsReader>();
            A.CallTo(() => ext1WordsReader.AllowedFileExtension).Returns(new Regex(@"\.ext1$"));

            ext2WordsReader = A.Fake<IWordsReader>();
            A.CallTo(() => ext2WordsReader.AllowedFileExtension).Returns(new Regex(@".ext2$"));

            reader = new GeneralWordsReader(new[] {ext1WordsReader, ext2WordsReader});
            generalWordsReader = reader;
        }

        [Test]
        public void ReturnError_WhenCantFindReaderForParticularFileExtension()
        {
            generalWordsReader.ReadFrom("file.with_wrong_format").IsSuccess.Should().BeFalse();
        }

        [Test]
        public void UseDifferentInnerReaders_OnFilesWithDifferentFormats()
        {
            const string pathForExt1 = "my_fake_words.ext1";
            const string pathForExt2 = "my_fake_words.ext2";

            generalWordsReader.ReadFrom(pathForExt1);
            generalWordsReader.ReadFrom(pathForExt2);

            A.CallTo(() => ext1WordsReader.ReadFrom(pathForExt1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => ext2WordsReader.ReadFrom(pathForExt2)).MustHaveHappenedOnceExactly();
        }
    }
}