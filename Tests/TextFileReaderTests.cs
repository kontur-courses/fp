using TagsCloudPainter.FileReader;
using TagsCloudPainter.Parser;

namespace TagsCloudPainterTests;

[TestFixture]
public class TextFileReaderTests
{
    [SetUp]
    public void Setup()
    {
        var fileReaders = new List<IFileReader> { new TxtFileReader(), new DocFileReader() };
        textFileReader = new TextFileReader(fileReaders);
        openedPath = @$"{Environment.CurrentDirectory}..\..\..\..\TestFiles\testFileOpened.txt";
        openedFile = File.Open(openedPath, FileMode.Open);
    }

    [TearDown]
    public void TearDown()
    {
        openedFile.Close();
    }

    private TextFileReader textFileReader;
    private string openedPath;
    private FileStream openedFile;

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenGivenNullFileReaders()
    {
        Assert.Throws<ArgumentNullException>(() => new TextFileReader(null));
    }

    private static IEnumerable<TestCaseData> ReadTextFiles => new[]
    {
        new TestCaseData(@$"{Environment.CurrentDirectory}..\..\..\..\TestFiles\testFile.txt")
            .Returns("Товарищи! постоянное информационно-пропагандистское обеспечение нашей " +
                     $"деятельности играет важную роль в формировании форм развития.{Environment.NewLine}{Environment.NewLine}" +
                     "Значимость этих проблем настолько очевидна, что укрепление и развитие.")
            .SetName("WhenPassedTxtFile"),
        new TestCaseData(@$"{Environment.CurrentDirectory}..\..\..\..\TestFiles\testFile.docx")
            .Returns("Товарищи! постоянное информационно-пропагандистское обеспечение нашей " +
                     $"деятельности играет важную роль в формировании форм развития.{Environment.NewLine}{Environment.NewLine}" +
                     "Значимость этих проблем настолько очевидна, что укрепление и развитие.")
            .SetName("WhenPassedDocxFile"),
        new TestCaseData(@$"{Environment.CurrentDirectory}..\..\..\..\TestFiles\testFile.doc")
            .Returns("Товарищи! постоянное информационно-пропагандистское обеспечение нашей " +
                     $"деятельности играет важную роль в формировании форм развития.{Environment.NewLine}{Environment.NewLine}" +
                     "Значимость этих проблем настолько очевидна, что укрепление и развитие.")
            .SetName("WhenPassedDocFile")
    };

    [TestCaseSource(nameof(ReadTextFiles))]
    public string ReadFile_ShouldReturnFileText(string path)
    {
        return textFileReader.ReadFile(path).GetValueOrThrow();
    }

    private static IEnumerable<TestCaseData> FailingFiles => new[]
    {
        new TestCaseData("").SetName("WhenGivenNonexistentFile"),
        new TestCaseData(@$"{Environment.CurrentDirectory}..\..\..\..\TestFiles\testFile.pdf")
        .SetName("WhenGivenFileWithUnsupportedExtension"),
        new TestCaseData(@$"{Environment.CurrentDirectory}..\..\..\..\TestFiles\image.png")
        .SetName("WhenGivenFileWithUnsupportedFormat"),
    };

    [TestCaseSource(nameof(FailingFiles))]
    public void ReadFile_Fails_(string file)
    {
        var result = textFileReader.ReadFile(file);

        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public void ReadFile_Fails_WhenGivenFileWithRestrictedAccess()
    {
        var result = textFileReader.ReadFile(openedPath);

        Assert.That(result.IsSuccess, Is.False);
    }
}