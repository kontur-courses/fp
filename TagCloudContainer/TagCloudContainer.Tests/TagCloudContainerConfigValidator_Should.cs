using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloudContainer.Configs;
using TagCloudContainer.Core;
using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Forms.Validators;
using TagCloudContainer.Utils;

namespace TagCloudContainer.Tests;

[TestFixture]
public class TagCloudContainerConfigValidator_Should
{
    private ITagCloudContainerConfig _tagCloudContainerConfig;
    private TagCloudContainerConfigValidator _tagCloudContainerConfigValidator;
    
    [SetUp]
    public void SetUp()
    {
        _tagCloudContainerConfigValidator = new TagCloudContainerConfigValidator();
        _tagCloudContainerConfig = new TagCloudContainerConfig()
        {
            NeedValidateWords = true,
            StandartFontSize = new Size(10, 10),
            ImageName = "image.png"
        };
        
        var wordsFilePath = PathAssistant.GetFullFilePath("words.txt");
        var excludeWordsFilePath = PathAssistant.GetFullFilePath("boring_words.txt");
        _tagCloudContainerConfig.WordsFilePath = wordsFilePath.IsSuccess ? wordsFilePath.GetValueOrThrow() : null;
        _tagCloudContainerConfig.ExcludeWordsFilePath = excludeWordsFilePath.IsSuccess ? excludeWordsFilePath.GetValueOrThrow() : null;
    }
    
    [Test]
    public void ValidateContainerConfig_CorrectParameters_ShouldBeOk()
    {
        _tagCloudContainerConfigValidator
            .Validate(_tagCloudContainerConfig)
            .IsSuccess
            .Should()
            .BeTrue();
    }
    
    [Test]
    public void ValidateContainerConfig_IncorrectWordsFilePath_ShouldBeFail()
    {
        _tagCloudContainerConfig.WordsFilePath = null;
        
       _tagCloudContainerConfigValidator
            .Validate(_tagCloudContainerConfig)
            .Should()
            .BeEquivalentTo(Result.Fail<ITagCloudContainerConfig>("Words file does not exist"));
    }
    
    [Test]
    public void ValidateContainerConfig_IncorrectExcludeWordsFilePath_ShouldBeFail()
    {
        _tagCloudContainerConfig.ExcludeWordsFilePath = null;
        _tagCloudContainerConfigValidator
            .Validate(_tagCloudContainerConfig)
            .Should()
            .BeEquivalentTo(Result.Fail<ITagCloudContainerConfig>("Exclude words file does not exist"));
    }
}