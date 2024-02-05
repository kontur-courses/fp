using NUnit.Framework;
using TagsCloud.Builders;

namespace TagsCloud.Tests;

[TestFixture]
[TestOf(nameof(CloudOptionsBuilder))]
public class CloudOptionsBuilderTests
{
    private const string badFontFile = "404.ttf";
    private const string badFontFamily = "mono-dotnet";

    private readonly CloudOptionsBuilder optionsBuilder = new();

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
}