using TagsCloudContainer;
using TagsCloudContainer.FileReader;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainerTests;

[TestFixture]
public class WordProcessingTest
{
    private FileReaderFactory reader;
    private Settings settings;
    private string projectDirectory;

    [SetUp]
    public void SetUp()
    {
        reader = new FileReaderFactory();
        settings = new Settings()
        {
            FontName = "Arial",
            FontSize = 14
        };
        projectDirectory = "../../../../TagsCloudContainerTests/TextFiles/";
    }
    
    [TestCase("UpperWords.txt")]
    [TestCase("UpperWords.docx")]
    public void WordProcessing_ShouldCorrectToLowerAndCountWords(string filename)
    {
        var testWords = new List<Word>()
        {
            new("привет") { Count = 3 },
            new("как") { Count = 5 },
            new("дела") { Count = 2 },
            new("у") { Count = 1 },
            new("тебя") { Count = 1 }
        };
        filename = projectDirectory + filename;
        var file = GetText(filename);
        var words = new WordProcessor(settings).ProcessWords(file);
        for (var i = 0; i < words.Count; i++)
            words[i].Count.Should().Be(testWords[i].Count);
    }
    
    [TestCase("Boring2.txt", "Boring.txt")]
    [TestCase("Boring2.docx", "Boring.docx")]
    public void WordProcessing_ShouldReturnEmptyArray(string filename, string boringFileName = "")
    {
        filename = projectDirectory + filename;
        boringFileName = projectDirectory + boringFileName;
        var fileText = GetText(filename);
        var boringText = GetText(boringFileName);
        var words = new WordProcessor(settings).ProcessWords(fileText, boringText);
        words.Should().BeEmpty();
    }

    [TestCase("UpperWords.txt")]
    [TestCase("UpperWords.docx")]
    public void WordProcessing_ShouldReturnWordsInLowerCase(string filename)
    {
        filename = projectDirectory + filename;
        var fileText = GetText(filename);
        var words = new WordProcessor(settings).ProcessWords(fileText);
        foreach (var t in words)
            t.Value.Should().Be(t.Value.ToLower());
    }

    private string GetText(string filename)
    {
        return reader.GetReader(filename).Value.GetTextFromFile(filename).Value;
    }
}
