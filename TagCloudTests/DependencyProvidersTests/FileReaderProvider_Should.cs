using FakeItEasy;
using TagCloud.FileReader;

namespace TagCloudTests.DependencyProvidersTest;

[TestFixture]
public class FileReaderProvider_Should
{
    private IFileReader reader;
    private IFileReaderProvider sut;
    private const string fileName = "test";
    private const string extension = "txt";

    [SetUp]
    public void SetUp()
    {
        reader = A.Fake<IFileReader>();
        A.CallTo(() => reader.GetAvailableExtensions()).Returns(new List<string>() { extension });
        sut = new FileReaderProvider(new List<IFileReader>() { reader });
    }

    [Test]
    public void CreateGenerator_WithCorrectName()
    {
        var result = sut.CreateReader($"{fileName}.{extension}");

        result.IsSuccess.Should().BeTrue();
        result.GetValueOrThrow().GetAvailableExtensions().Should().Contain(extension);
    }

    [Test]
    public void ReturnFail_OnWrongGeneratorName()
    {
        var wrongExtension = extension + "x";

        var result = sut.CreateReader($"{fileName}.{wrongExtension}");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should()
            .Be(
                $"Reading of file {fileName}.{wrongExtension} with extension {wrongExtension} is not supported. Available file formats are: {extension}");
    }
}