using NUnit.Framework;
using SixLabors.ImageSharp;
using TagsCloud.Entities;
using TagsCloud.FontMeasurers;
using TagsCloud.Options;
using TagsCloud.Painters;
using TagsCloud.Processors;
using TagsCloudVisualization;

namespace TagsCloud.Tests;

[TestFixture]
[TestOf(nameof(CloudProcessor))]
public class CloudProcessorTests
{
    [SetUp]
    public void SetUp()
    {
        var cloudOptions = new CloudProcessorOptions
        {
            Colors = colors,
            ColoringStrategy = ColoringStrategy.OneVsRest,
            MeasurerType = MeasurerType.Linear
        };

        processor = new CloudProcessor(cloudOptions, new[] { painter }, new[] { measurer });
    }

    private readonly IPainter painter = new OneVsRestPainter();
    private readonly IFontMeasurer measurer = new LinearFontMeasurer();
    private readonly Color[] colors = { Color.White, Color.Blue, Color.Red };

    private ICloudProcessor processor;

    [Test]
    public void Processor_Should_ReturnFailResult_When_ColorsCountIsBadForPainter()
    {
        var processorResult = processor
            .SetColors(new HashSet<WordTagGroup>());

        TestHelper.AssertResultFailAndErrorText(
            processorResult,
            $"Must be exactly 2 colors for {nameof(OneVsRestPainter)}!");
    }
}