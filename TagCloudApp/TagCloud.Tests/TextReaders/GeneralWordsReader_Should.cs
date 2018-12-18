using System.IO;
using System.Text.RegularExpressions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Core.WordsParsing.WordsReading;

namespace TagCloud.Tests.TextReaders
{
    [TestFixture]
    public class GeneralWordsReader_Should
    {
        private string resourcesDir;
        private GeneralWordsReader generalWordsReader;
        private IWordsReader ext1WordsReader;
        private IWordsReader ext2WordsReader;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            resourcesDir = TestContext.CurrentContext.TestDirectory + @"\..\..\Resources\fake\";
            
            ext1WordsReader = A.Fake<IWordsReader>();
            A.CallTo(() => ext1WordsReader.AllowedFileExtension).Returns(new Regex(@"\.ext1$"));

            ext2WordsReader = A.Fake<IWordsReader>();
            A.CallTo(() => ext2WordsReader.AllowedFileExtension).Returns(new Regex(@"\.ext2$"));

            generalWordsReader = new GeneralWordsReader(new[] {ext1WordsReader, ext2WordsReader});
        }

        [Test]
        public void ReturnError_WhenCantFindReaderForParticularFileExtension()
        {
            generalWordsReader.ReadFrom("file.with_wrong_format").IsSuccess.Should().BeFalse();
        }

        [Test]
        public void UseDifferentInnerReaders_OnFilesWithDifferentFormats()
        {
            const string fileForExt1 = "fake_file.ext1";
            const string fileForExt2 = "fake_file.ext2";

            generalWordsReader.ReadFrom(resourcesDir + fileForExt1);
            generalWordsReader.ReadFrom(resourcesDir + fileForExt2);

            A.CallTo(() => ext1WordsReader.ReadFrom(A<Stream>.Ignored))
                .WithAnyArguments()
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => ext2WordsReader.ReadFrom(A<Stream>.Ignored))
                .WithAnyArguments()
                .MustHaveHappenedOnceExactly();
        }
    }
}