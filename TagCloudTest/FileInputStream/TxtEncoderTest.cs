using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.App.WordPreprocessorDriver.InputStream;

namespace TagCloudTest.FileInputStream;

public class TxtEncoderTest
{
    private static string? _filePath;

    [OneTimeSetUp]
    public void StartTests()
    {
        _filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "test.txt";
    }

    // If you run all the tests at once, then it crashes. If you only run it, then everything works
    [Test]
    public void GetText_ShouldReturnTextFromTxtFile_WhenOnlyOneLineInFile()
    {
        const string text = "word1 word2";
        GetText_Helper(text, CreateFileWithText(text));
    }

    // If you run all the tests at once, then it crashes. If you only run it, then everything works
    [Test]
    public void GetText_ShouldReturnTextFromTxtFile_WhenManyLinesInFile()
    {
        var text = "text1" + Environment.NewLine + "text2";
        GetText_Helper(text, CreateFileWithText(text));
    }

    [TearDown]
    public void OnTestStop()
    {
        try
        {
            File.Delete(_filePath!);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private static string CreateFileWithText(string text)
    {
        File.WriteAllText(_filePath!, text);
        return _filePath!;
    }

    private static void GetText_Helper(string expectedText, string filePath)
    {
        var sut = new TxtEncoder();
        var result = sut.GetText(filePath);
        result.IsSuccess.Should().BeTrue();
        result.GetValueOrThrow()!.Should().Be(expectedText);
    }
}