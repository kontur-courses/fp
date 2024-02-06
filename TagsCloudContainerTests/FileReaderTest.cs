using TagsCloudContainer.FileReader;
namespace TagsCloudContainerTests
{
    [TestFixture]
    public class FileReaderTest
    {
        //Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\TextFiles\\" + fileName

        [TestCase("Text.txt")]
        [TestCase("Boring.txt")]
        public void GetReader_ShouldCorrectTxtReader(string fileName)
        {
            var reader = new FileReaderFactory().GetReader(fileName).Value;
            reader.Should().BeOfType<TxtFileReader>();
        }
        
        [TestCase("Text.docx")]
        [TestCase("Boring.docx")]
        public void GetReader_ShouldCorrectDocxReader(string fileName)
        {
            var reader = new FileReaderFactory().GetReader(fileName).Value;
            reader.Should().BeOfType<DocxFileReader>();
        }
        
        [TestCase("Text.mp4")]
        [TestCase("Boring.cs")]
        public void GetReader_ShouldThrowArgumentException(string fileName)
        {
            var result = new FileReaderFactory().GetReader(fileName);
            result.Error.Should().Be($"Неверный формат файла: {Path.GetExtension(fileName)}");
        }
    }
}