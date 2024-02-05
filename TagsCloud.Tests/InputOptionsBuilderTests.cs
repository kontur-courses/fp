using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Builders;

namespace TagsCloud.Tests;

[TestFixture]
[TestOf(nameof(InputOptionsBuilder))]
public class InputOptionsBuilderTests
{
    private readonly InputOptionsBuilder optionsBuilder = new();

    [Test]
    public void Builder_Should_ReturnFailResult_When_BadTextParts()
    {
        var textParts = new HashSet<string> { "S", "V", "CORS", "RPC", "MVC" };

        var builderResult = optionsBuilder
            .SetLanguageParts(textParts);

        TestHelper.AssertResultFail(builderResult);
        builderResult.Error.Should().Contain("CORS, RPC, MVC");
        builderResult.Error.Should().NotContain("S, V");
    }
}