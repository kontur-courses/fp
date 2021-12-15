using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Infrastructure.FileReader;

namespace TagCloudTests.Infrastructure.FileReader;

internal class DocFileReaderTests
{
    private const string FileName = $"{nameof(DocFileReader)}Test.docx";
    private const string InvalidFileName = $"{nameof(DocFileReader)}Test.txt";
    private readonly DocFileReader sut = new();

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        if (!File.Exists(InvalidFileName))
            File.Create(InvalidFileName).Dispose();

        var paragraphs = new[] { "firstline", "secondline", "thirdline", "" };
        using var document = WordprocessingDocument.Create(FileName, WordprocessingDocumentType.Document, true);
        var mainPart = document.AddMainDocumentPart();
        mainPart.Document = new Document();
        var body = new Body();

        foreach (var paragraph in paragraphs)
        {
            var run = new Run();
            run.Append(new Text(paragraph));
            body.Append(new Paragraph(run));
        }

        mainPart.Document.Append(body);
    }

    [Test]
    public void GetLines_ShouldReturnCorrectLinesEnumerable()
    {
        var expected = new[] { "firstline", "secondline", "thirdline", "" };

        var actual = sut.GetLines(FileName);

        actual.GetValueOrThrow().Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetLines_ShouldReturnFail_WhenFileDoesNotExists()
    {
        const string path = "notExistingFile.docx";

        var actual = sut.GetLines(path);

        actual.IsSuccess.Should().BeFalse();
        actual.Error.Should().Be($"The file does not exist {path}");
    }

    [Test]
    public void GetLines_ShouldReturnFail_WhenFileHasUnsupportedExtension()
    {
        var actual = sut.GetLines(InvalidFileName);

        actual.IsSuccess.Should().BeFalse();
        actual.Error.Should().Be($"Extension {new FileInfo(InvalidFileName).Extension} is not supported");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (File.Exists(InvalidFileName))
            File.Delete(InvalidFileName);
    }
}