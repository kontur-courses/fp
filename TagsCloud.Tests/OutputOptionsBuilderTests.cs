using NUnit.Framework;
using TagsCloud.Builders;

namespace TagsCloud.Tests;

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
}