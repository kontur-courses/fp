using FakeItEasy;
using NUnit.Framework;
using SixLabors.ImageSharp.Formats;
using TagsCloud.Options;
using TagsCloud.Processors;
using TagsCloudVisualization;

namespace TagsCloud.Tests;

[TestFixture]
[TestOf(nameof(OutputProcessor))]
public class OutputProcessorTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        A.CallTo(() => fakeBuilder.SaveAs(A<string>._, A<IImageEncoder>._))
         .Throws<Exception>();
    }

    private readonly IOutputProcessor processor = new OutputProcessor(new OutputProcessorOptions());
    private readonly IVisualizationBuilder fakeBuilder = A.Fake<IVisualizationBuilder>();

    [Test]
    public void Processor_Should_ReturnFailResult_WhenFileSavingError()
    {
        var processorResult = processor
            .SaveVisualization(new HashSet<WordTagGroup>(), "image.png");

        TestHelper.AssertResultFailAndErrorText(
            processorResult,
            "Can't save image! Please, check app permissions.");
    }
}