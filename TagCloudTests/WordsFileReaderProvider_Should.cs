using TagCloudCore.Domain.Providers;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;
using TagCloudCore.Interfaces.Settings;
using TagCloudCoreExtensions.WordsFileReaders;

namespace TagCloudTests;

[TestFixture]
public class WordsFileReaderProvider_Should
{
    private IFileReaderProvider _readerProvider = null!;
    private IWordsPathSettings _pathSettings = null!;

    [SetUp]
    public void Setup()
    {
        _pathSettings = A.Fake<IWordsPathSettings>();
        var pathSettingsProvider = A.Fake<IWordsPathSettingsProvider>();
        A.CallTo(() => pathSettingsProvider.GetWordsPathSettings())
            .Returns(_pathSettings);

        _readerProvider = new FileReaderProvider(
            new IFileReader[]
            {
                new TxtFileReader(pathSettingsProvider),
                new DocxFileReader(pathSettingsProvider)
            },
            pathSettingsProvider
        );
    }

    [TestCase(".txt", typeof(TxtFileReader), TestName = "Txt")]
    [TestCase(".docx", typeof(DocxFileReader), TestName = "Docx")]
    public void ReturnCorrectReader_ForCorrectExtension(string extension, Type expectedReaderType)
    {
        _pathSettings.WordsPath = $"text{extension}";
        _readerProvider.GetReader().GetValueOrThrow().GetType()
            .Should().Be(expectedReaderType);
    }

    [Test]
    public void Fail_ForBadExtension()
    {
        _pathSettings.WordsPath = "text.abc";
        var saverResult = _readerProvider.GetReader();
        saverResult.IsSuccess.Should().BeFalse();
        saverResult.Error.Should().Be("No reader for extension: .abc");
    }
}