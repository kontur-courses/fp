using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.App;

namespace TagsCloudTests
{
    [TestFixture]
    public class TxtFileReaderTests
    {
        private readonly TxtFileReader reader = new TxtFileReader();

        [Test]
        public void TxtFileReader_ShouldThrow_WithWrongFileType()
        {
            var fileName = @"C:\Users\da\Desktop\abc.jpeg";
            var resultOfReading = reader.ReadWords(fileName);
            resultOfReading.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void TxtFileReader_ShouldReadLines_FromDocTypeFile()
        {
            var fileName = Directory.GetCurrentDirectory() + @"\FileReadersTestsFiles\TxtFileReaderTestFile.txt";
            TestContext.WriteLine(fileName);
            var words = reader.ReadWords(fileName).Value;
            words.Should().BeEquivalentTo("Abc", "Aa", "Abcg", "Def", "Gf");
        }

        [Test]
        public void TxtFileReader_ShouldReturnEmptyCollection_FromEmptyDocFile()
        {
            var fileName = Directory.GetCurrentDirectory() + @"\FileReadersTestsFiles\EmptyTxtFile.txt";
            TestContext.WriteLine(fileName);
            var words = reader.ReadWords(fileName).Value;
            words.Should().BeEquivalentTo();
        }
    }
}