using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloudPainter.FileReader;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Tests;

public class FileReadersTests
{
    private List<string> wordList;
    public TxtReader TxtReader { get; set; }
    public DocxReader DocxReader { get; set; }
    public PdfFileReader PdfReader { get; set; }
    private string directoryPath { get; set; }

    [SetUp]
    public void Setup()
    {
        TxtReader = new TxtReader();
        DocxReader = new DocxReader();
        PdfReader = new PdfFileReader();
        wordList = "Бегал Красивый Умный Бывший Когда Сегодня Вчера при Привет Алексей".Split(' ').ToList();
        directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles");
    }

    [Test]
    public void TxtReader_Should_Fail_On_InvalidFormat()
    {
        var path = Path.Combine(directoryPath, "Test.pdf");
        var result = TxtReader.ReadFile(path);

        result.Should().BeEquivalentTo(Result.Fail<IEnumerable<string>>("file is not in txt format"));
    }

    [Test]
    public void TxtReader_Should_Fail_If_path_does_not_exist()
    {
        var path = Path.Combine(directoryPath, "abrakadabra.txt");
        var result = TxtReader.ReadFile(path);

        result.Should().BeEquivalentTo(Result.Fail<IEnumerable<string>>($"path {path} does not exist "));
    }

    [Test]
    public void TxtReader_BaseFunctionalityTest()
    {
        var path = Path.Combine(directoryPath, "Test.txt");
        var result = TxtReader.ReadFile(path).GetValueOrThrow();

        wordList.Should().BeEquivalentTo(result.ToList());
    }

    [Test]
    public void PdfReader_Should_Fail_On_InvalidFormat()
    {
        var path = Path.Combine(directoryPath, "Test.txt");
        var result = PdfReader.ReadFile(path);

        result.Should().BeEquivalentTo(Result.Fail<IEnumerable<string>>("file is not in pdf format"));
    }

    [Test]
    public void PdfReader_Should_Fail_If_path_does_not_exist()
    {
        var path = Path.Combine(directoryPath, "abrakadabra.pdf");
        var result = PdfReader.ReadFile(path);

        result.Should().BeEquivalentTo(Result.Fail<IEnumerable<string>>($"path {path} does not exist "));
    }

    [Test]
    public void PdfReader_BaseFunctionalityTest()
    {
        var path = Path.Combine(directoryPath, "Test.pdf");
        var result = PdfReader.ReadFile(path).GetValueOrThrow();

        wordList.Should().BeEquivalentTo(result.ToList());
    }

    [Test]
    public void DocxReader_Should_Fail_On_InvalidFormat()
    {
        var path = Path.Combine(directoryPath, "Test.txt");
        var result = DocxReader.ReadFile(path);

        result.Should().BeEquivalentTo(Result.Fail<IEnumerable<string>>("file is not in docx format"));
    }

    [Test]
    public void DocxReader_Should_Fail_If_path_does_not_exist()
    {
        var path = Path.Combine(directoryPath, "abrakadabra.docx");
        var result = DocxReader.ReadFile(path);

        result.Should().BeEquivalentTo(Result.Fail<IEnumerable<string>>($"path {path} does not exist "));
    }

    [Test]
    public void DocxReader_BaseFunctionalityTest()
    {
        var path = Path.Combine(directoryPath, "Test.docx");
        var result = DocxReader.ReadFile(path).GetValueOrThrow();

        wordList.Should().BeEquivalentTo(result.ToList());
    }
}