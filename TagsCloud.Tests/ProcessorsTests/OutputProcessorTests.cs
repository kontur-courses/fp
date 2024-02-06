using NUnit.Framework;
using SixLabors.ImageSharp;
using TagsCloud.Options;
using TagsCloud.Processors;
using TagsCloudVisualization;
using static TagsCloud.Tests.TestConfiguration;

namespace TagsCloud.Tests.ProcessorsTests;

[TestFixture]
[TestOf(nameof(OutputProcessor))]
public class OutputProcessorTests
{
    private const string notFoundDirectory = "NotFound";

    private readonly IOutputProcessor processor =
        new OutputProcessor(
            new OutputProcessorOptions
            {
                ImageSize = new Size(WindowWidth, WindowHeight)
            });

    [Test]
    public void Processor_Should_ReturnFailResult_WhenSavingDirectoryNotFound()
    {
        var testPath = Path.Combine(notFoundDirectory, Path.GetRandomFileName());
        var processorResult = processor
            .SaveVisualization(new HashSet<WordTagGroup>(), testPath);

        TestHelper.AssertResultFailAndErrorText(
            processorResult,
            $"Directory {notFoundDirectory} doesn't exist!");
    }
}