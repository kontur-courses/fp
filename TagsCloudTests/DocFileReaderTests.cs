using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.App;

namespace TagsCloudTests
{
    [TestFixture]
    public class DocFileReaderTests
    {
        private readonly DocFileReader reader = new DocFileReader();

        [Test]
        public void DocFileReader_ShouldThrow_WithWrongFileType()
        {
            var fileName = @"C:\Users\da\Desktop\abc.hguit";
            var result = reader.ReadWords(fileName);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void DocFileReader_ShouldReadLines_FromDocTypeFile()
        {
            var fileName = Directory.GetCurrentDirectory() + @"\FileReadersTestsFiles\DocFileReaderTestFile.docx";
            var words = reader.ReadWords(fileName).Value;
            words.Should().BeEquivalentTo("Abc", "Aa", "Abcg", "Def", "Gf");
        }

        [Test]
        public void DocFileReader_ShouldReturnEmptyCollection_FromEmptyDocFile()
        {
            var fileName = Directory.GetCurrentDirectory() + @"\FileReadersTestsFiles\EmptyDocFile.docx";
            var words = reader.ReadWords(fileName).Value;
            words.Should().BeEquivalentTo();
        }
    }
}