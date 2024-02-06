using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using TagsCloud.Builders;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Options;
using static TagsCloud.Tests.TestConfiguration;

namespace TagsCloud.Tests.BuildersTests;

[TestFixture]
[TestOf(nameof(OutputOptionsBuilder))]
public class OutputOptionsBuilderTests
{
    private readonly OutputOptionsBuilder optionsBuilder = new();

    [TestCase(1920, -1080)]
    [TestCase(-1920, 1080)]
    public void Builder_Should_ReturnFailResult_When_ImageWidthOrHeightBelowZero(int width, int height)
    {
        var builderResult = optionsBuilder
            .SetImageSize(width, height);

        TestHelper.AssertResultFailAndErrorText(builderResult);
    }

    [Test]
    public void Builder_Should_ReturnSuccessResult_When_CorrectInputValues()
    {
        var builderResult = optionsBuilder
                            .AsResult()
                            .Then(builder => builder.SetImageFormat(ImageFormat.Bmp))
                            .Then(builder => builder.SetImageSize(WindowWidth, WindowHeight))
                            .Then(builder => builder.SetImageBackgroundColor("#FFD700"))
                            .Then(builder => builder.BuildOptions());

        var expected = new OutputProcessorOptions
        {
            BackgroundColor = Color.Gold,
            ImageSize = new Size(WindowWidth, WindowHeight),
            ImageEncoder = new BmpEncoder()
        };

        TestHelper.AssertResultSuccess(builderResult);
        builderResult.Value.ShouldBeEquivalentTo(expected,
            opt
                => opt.Using<Color>(ctx => ctx.Subject.Should().Be(ctx.Expectation))
                      .WhenTypeIs<Color>()
                      .Using<IImageEncoder>(ctx =>
                          ctx.Subject.GetType().Should().Be(ctx.Expectation.GetType()))
                      .WhenTypeIs<IImageEncoder>());
    }
}