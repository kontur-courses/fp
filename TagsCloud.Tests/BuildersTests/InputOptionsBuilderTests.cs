using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Builders;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Options;

namespace TagsCloud.Tests.BuildersTests;

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

    [Test]
    public void Builder_Should_ReturnSuccessResult_When_CorrectInputValues()
    {
        var builderResult = optionsBuilder
                            .AsResult()
                            .Then(builder => builder.SetCastPolitics(true))
                            .Then(builder => builder.SetWordsCase(CaseType.Lower))
                            .Then(builder => builder.SetLanguagePolitics(false))
                            .Then(builder => builder.SetExcludedWords(new HashSet<string> { "C++" }))
                            .Then(builder => builder.SetLanguageParts(new HashSet<string> { "S", "CONJ" }))
                            .Then(builder => builder.BuildOptions());

        var expected = new InputProcessorOptions
        {
            WordsCase = CaseType.Lower,
            ToInfinitive = true,
            ExcludedWords = new HashSet<string> { "C++" },
            LanguageParts = new HashSet<string> { "S", "CONJ" },
            OnlyRussian = false
        };

        TestHelper.AssertResultSuccess(builderResult);
        builderResult.Value.ShouldBeEquivalentTo(expected);
    }
}