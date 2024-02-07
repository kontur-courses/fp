using TagCloud.FileReader;

namespace TagCloudTests;

[TestFixture]
public class FileReader_Should
{
    private IFileReader sut = new TxtReader();
    private const string inputPath = "test.txt";
    private string text = $"one{Environment.NewLine}two{Environment.NewLine}three{Environment.NewLine}";

    [Test]
    public void ReadWordsFromTxt()
    {
        using var fileStream = File.Open(inputPath, FileMode.Create);
        using var writer = new StreamWriter(fileStream);
        writer.Write(text);
        writer.Close();

        var expected = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();

        var result = sut.ReadLines(inputPath);

        result.IsSuccess.Should().BeTrue();
        result.GetValueOrThrow().Should().BeEquivalentTo(expected);

        File.Delete(inputPath);
    }

    [Test]
    public void ReturnFail_WhenFileDoesntExist()
    {
        var path = "FileReaderTest.txt";

        if (File.Exists(path))
            File.Delete(path);

        var result = sut.ReadLines(path);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo($"Input file {path} doesn't exist");
    }
}