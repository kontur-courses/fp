using FakeItEasy;
using NUnit.Framework;
using TagsCloud.FileReaders;
using TagsCloud.Filters;
using TagsCloud.Formatters;
using TagsCloud.Options;
using TagsCloud.Processors;

namespace TagsCloud.Tests;

[TestFixture]
[TestOf(nameof(InputProcessor))]
public class InputProcessorTests
{
    [SetUp]
    public void SetUp()
    {
        A.CallTo(() => fakeReader.ReadContent(A<string>._, defaultFormatter))
         .Returns(new[] { string.Empty });
        A.CallTo(() => fakeReader.SupportedExtension)
         .Returns("txt");

        processor = new InputProcessor(
            new InputProcessorOptions(),
            new[] { fakeReader },
            new[] { fakeFilter },
            defaultFormatter);
    }

    private const string badFileName = "find-me.txt";
    private const string badExtFileName = "error_data.gps";
    private const string goodFileName = "data.txt";

    private readonly string pathToTestData = Path.Join(Directory.GetCurrentDirectory(), "TestData");
    private readonly IPostFormatter defaultFormatter = new DefaultPostFormatter();
    private readonly IFileReader fakeReader = A.Fake<IFileReader>();
    private readonly IFilter fakeFilter = A.Fake<IFilter>();

    private IInputProcessor processor;

    [Test]
    public void Processor_Should_ReturnFailResult_When_WordsFileNotFound()
    {
        var processorResult = processor
            .CollectWordGroupsFromFile(badFileName);

        TestHelper.AssertResultFailAndErrorText(processorResult, $"File {badFileName} doesn't exist");
    }

    [Test]
    public void Processor_Should_ReturnFailResult_When_UnknownFileExtension()
    {
        var processorResult = processor
            .CollectWordGroupsFromFile(Path.Join(pathToTestData, badExtFileName));

        TestHelper.AssertResultFailAndErrorText(processorResult, "Unknown file extension: gps!");
    }

    [Test]
    public void Processor_Should_ReturnFailResult_When_ZeroWordsGroups()
    {
        var processorResult = processor
            .CollectWordGroupsFromFile(Path.Join(pathToTestData, goodFileName));

        TestHelper.AssertResultFailAndErrorText(processorResult, "Can't generate TagCloud from void!");
    }

    [Test]
    public void Processor_Should_ReturnFailResult_When_FileReadingError()
    {
        A.CallTo(() => fakeReader.ReadContent(A<string>._, defaultFormatter))
         .Throws<Exception>();

        var processorResult = processor
            .CollectWordGroupsFromFile(Path.Join(pathToTestData, goodFileName));

        TestHelper.AssertResultFailAndErrorText(processorResult, "Can't read file content");
    }
}