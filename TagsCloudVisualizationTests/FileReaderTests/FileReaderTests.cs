using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.FileReaders;

namespace TagsCloudVisualizationTests.FileReaderTests;

[TestFixture]
public class FileReaderTests
{
    [Test]
    public void ReadText_FileNotExist_ReturnsFalse()
    {
        var reader = new TxtReader();
        var result = reader.ReadText(@"..\..\..\FileReaderData\NotExistFile.txt");
        result.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void ReadText_TxtFile_ReturnTextInFile()
    {
        var reader = new TxtReader();
        var result = reader.ReadText(@"..\..\..\FileReaderData\TxtFile.txt");
        result.GetValueOrThrow().Should().Be("txt\r\nfile\r\ndata");
    }

    [Test]
    public void ReadText_DocFile_ReturnsTextInFile()
    {
        var reader = new DocReader();
        var result = reader.ReadText(@"..\..\..\FileReaderData\DocFile.doc");
        result.GetValueOrThrow().Should().Be("doc file data");
    }

    [Test]
    public void ReadText_DocxFile_ReturnsTextInFile()
    {
        var reader = new DocxReader();
        var result = reader.ReadText(@"..\..\..\FileReaderData\DocxFile.docx");
        result.GetValueOrThrow().Should().Be("docx file data");
    }

    [TestCase("Empty.docx", " ", TestName = "empty docx file")]
    [TestCase("Empty.txt", "", TestName = "empty txt file")]
    [TestCase("Empty.doc", "", TestName = "empty doc file")]
    public void ReadText_EmptyTxtFile_ReturnsEmptyString(string fileName, string exceptedResult)
    {
        IFileReader reader = fileName.Split('.')[^1] switch
        {
            "docx" => new DocxReader(),
            "txt" => new TxtReader(),
            "doc" => new DocReader(),
            _ => throw new ArgumentException()
        };
        var result = reader.ReadText($@"..\..\..\FileReaderData\{fileName}");
        result.GetValueOrThrow().Should().Be(exceptedResult);
    }
}