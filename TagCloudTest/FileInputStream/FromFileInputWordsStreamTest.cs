using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.App.WordPreprocessorDriver.InputStream;
using TagCloud.App.WordPreprocessorDriver.InputStream.TextSplitters;
using TagCloudTest.FileInputStream.Infrastructure;

namespace TagCloudTest.FileInputStream;

public class FromFileInputWordsStreamTest
{
    private readonly string textWithNewLines = "word1" + Environment.NewLine + "word2" + Environment.NewLine + "word3";
    private string path = "test.txt";
    private readonly FromFileInputWordsStream sut = new(new NewLineTextSplitter());

    [OneTimeSetUp]
    public void StartTests()
    {
        path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "test.txt";
        File.Create(path);
    }

    [Test]
    public void GetAllWordsFromStream_ShouldReturnEmptyList_WhenNoWords()
    {
        var result = sut.GetAllWordsFromStream(GetContext(""));
        result.IsSuccess.Should().BeTrue();
        result.GetValueOrThrow()!.Should().BeEmpty();
    }

    [Test]
    public void GetAllWordsFromStream_ShouldThrowFileNotFoundException_WhenIncorrectFilename()
    {
        sut.GetAllWordsFromStream(GetContext("", "incorrectPath"))
            .IsSuccess.Should().BeFalse();
    }

    [Test]
    public void GetAllWordsFromStream_ShouldThrowException_WhenIncorrectFileType()
    {
        sut.GetAllWordsFromStream(GetContext("", filepath: "incorrect", filetype: "docx"))
            .IsSuccess.Should().BeFalse();
    }

    [Test]
    public void GetAllWordsFromStream_ShouldReturnAllWordsFromFile()
    {
        var words = sut.GetAllWordsFromStream(GetContext(textWithNewLines));
        words.IsSuccess.Should().BeTrue();
        words.GetValueOrThrow()!.Count.Should().Be(3);
    }


    [OneTimeTearDown]
    public void StopTests()
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private FromFileStreamContext GetContext(string textInFile, string? filepath = null, string filetype = "txt")
    {
        filepath ??= path;
        return new FromFileStreamContext(filepath, new FileEncoderСheater(textInFile, true, filetype));
    }
}