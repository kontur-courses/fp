using FluentAssertions;
using NUnit.Framework;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using TagsCloud.Builders;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Options;
using TagsCloudVisualization;
using static TagsCloud.Tests.TestConfiguration;

namespace TagsCloud.Tests.BuildersTests;

[TestFixture]
[TestOf(nameof(CloudOptionsBuilder))]
public class CloudOptionsBuilderTests
{
    [SetUp]
    public void SetUp()
    {
        optionsBuilder = new CloudOptionsBuilder();
    }

    private const string badFontFile = "404.ttf";
    private const string badFontFamily = "mono-dotnet";
    private const string fontName = "Vollkorn";

    private CloudOptionsBuilder optionsBuilder;

    [TestCase(0, 100)]
    [TestCase(100, 0)]
    public void Builder_Should_ReturnFailResult_When_FontBoundsLessOrEqualToZero(int lowerBound, int upperBound)
    {
        var builderResult = optionsBuilder
            .SetFontBounds(lowerBound, upperBound);

        TestHelper.AssertResultFail(builderResult);
    }

    [TestCase(150, 100)]
    [TestCase(100, 100)]
    public void Builder_Should_ReturnFailResult_When_UpperFontBoundLessOrEqualToLower(int lowerBound, int upperBound)
    {
        var builderResult = optionsBuilder
            .SetFontBounds(lowerBound, upperBound);

        TestHelper.AssertResultFail(builderResult);
    }

    [Test]
    public void Builder_Should_ReturnFailResult_When_FontFileNotFound()
    {
        var builderResult = optionsBuilder
            .SetFontFamily(badFontFile);

        TestHelper.AssertResultFailAndErrorText(builderResult, $"{badFontFile} font file not found!");
    }

    [Test]
    public void Builder_Should_ReturnFailResult_When_FontFamilyNotFoundInSystem()
    {
        var builderResult = optionsBuilder
            .SetFontFamily(badFontFamily);

        TestHelper.AssertResultFailAndErrorText(builderResult, $"{badFontFamily} family is unknown!");
    }

    [Test]
    public void Builder_Should_ReturnFailResult_When_FontFamilyNotSet()
    {
        var builderResult = optionsBuilder
                            .AsResult()
                            .Then(builder => builder
                                .SetLayout(PointGeneratorType.Spiral, new PointF(), 0, 0))
                            .Then(builder => builder.BuildOptions());

        TestHelper.AssertResultFailAndErrorText(
            builderResult,
            $"You must set {nameof(FontFamily)} before building options!");
    }

    [Test]
    public void Builder_Should_ReturnFailResult_When_LayoutNotSet()
    {
        var builderResult = optionsBuilder
                            .AsResult()
                            .Then(builder => builder.SetFontFamily(Path.Join(TestDataPath, $"{fontName}-Regular.ttf")))
                            .Then(builder => builder.BuildOptions());

        TestHelper.AssertResultFailAndErrorText(
            builderResult,
            $"You must set {nameof(Layout)} before building options!");
    }

    [Test]
    public void Builder_Should_ReturnSuccessResult_When_CorrectInputValues()
    {
        var builderResult = optionsBuilder
                            .AsResult()
                            .Then(builder => builder.SetSortingType(SortType.Preserve))
                            .Then(builder => builder.SetMeasurerType(MeasurerType.Logarithmic))
                            .Then(builder => builder.SetColoringStrategy(ColoringStrategy.AllRandom))
                            .Then(builder => builder.SetColors(new HashSet<string> { "#FFD700" }))
                            .Then(builder => builder.SetFontBounds(40, 50))
                            .Then(builder => builder.SetFontFamily(
                                Path.Combine(TestDataPath, $"{fontName}-Regular.ttf")))
                            .Then(builder => builder
                                .SetLayout(PointGeneratorType.Spiral, new PointF(), 0, 0))
                            .Then(builder => builder.BuildOptions());

        var expected = new CloudProcessorOptions
        {
            Colors = new[] { Color.Gold },
            MinFontSize = 40,
            MaxFontSize = 50,
            ColoringStrategy = ColoringStrategy.AllRandom,
            MeasurerType = MeasurerType.Logarithmic,
            Sort = SortType.Preserve
        };

        TestHelper.AssertResultSuccess(builderResult);
        builderResult.Value.ShouldBeEquivalentTo(expected,
            config =>
                config.Excluding(options => options.Layout)
                      .Using<FontFamily>(ctx =>
                          ctx.Subject.Name.Should().Be(fontName))
                      .WhenTypeIs<FontFamily>());
    }
}