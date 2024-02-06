using NUnit.Framework;
using SixLabors.ImageSharp;
using TagsCloud.Entities;
using TagsCloud.FontMeasurers;
using TagsCloud.Options;
using TagsCloud.Painters;
using TagsCloud.Processors;
using TagsCloudVisualization;

namespace TagsCloud.Tests.ProcessorsTests;

[TestFixture]
[TestOf(nameof(CloudProcessor))]
public class CloudProcessorTests
{
    private readonly IPainter oneVsRestPainter = new OneVsRestPainter();
    private readonly Color[] colors = { Color.White, Color.Blue, Color.Red };

    [Test]
    public void Processor_Should_ReturnFailResult_When_ColorsCountIsBadForPainter()
    {
        var cloudOptions = new CloudProcessorOptions
        {
            Colors = colors,
            ColoringStrategy = ColoringStrategy.OneVsRest,
            MeasurerType = MeasurerType.Linear
        };

        var processor = new CloudProcessor(
            cloudOptions,
            new[] { oneVsRestPainter },
            Array.Empty<IFontMeasurer>());

        var processorResult = processor
            .SetColors(new HashSet<WordTagGroup>());

        TestHelper.AssertResultFailAndErrorText(
            processorResult,
            $"Must be exactly 2 colors for {nameof(OneVsRestPainter)}!");
    }
}