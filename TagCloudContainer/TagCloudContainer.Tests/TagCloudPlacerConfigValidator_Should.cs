using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloudContainer.Configs;
using TagCloudContainer.Core;
using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Forms.Validators;

namespace TagCloudContainer.Tests;

[TestFixture]
public class TagCloudFormConfigValidator_Should
{
    private ITagCloudPlacerConfig _tagCloudPlacerConfig;
    private TagCloudPlacerConfigValidator _tagCloudPlacerConfigValidator;
    
    [SetUp]
    public void SetUp()
    {
        _tagCloudPlacerConfigValidator = new TagCloudPlacerConfigValidator();        
        _tagCloudPlacerConfig = new TagCloudPlacerConfig()
        {
            FieldCenter = new Point(50, 50),
            NearestToTheFieldCenterPoints = new SortedList<float, Point>(),
            PutRectangles = new List<Rectangle>()
        };
    }
    
    [Test]
    public void ValidateTagCloudFormConfig_CorrectParameters_ShouldBeOk()
    {
        _tagCloudPlacerConfigValidator
            .Validate(_tagCloudPlacerConfig)
            .IsSuccess
            .Should()
            .BeTrue();
    }
    
    [Test]
    public void ValidateTagCloudFormConfig_EmptyFieldCenter_ShouldBeFail()
    {
        _tagCloudPlacerConfig.FieldCenter = Point.Empty;
        _tagCloudPlacerConfigValidator 
            .Validate(_tagCloudPlacerConfig)
            .Should()
            .BeEquivalentTo(Result.Fail<ITagCloudPlacerConfig>("Field center point can't be empty or null"));
    }
    
    [Test]
    public void ValidateTagCloudFormConfig_NullablePutRectanglesList_ShouldBeFail()
    {
        _tagCloudPlacerConfig.PutRectangles = null;
        _tagCloudPlacerConfigValidator 
            .Validate(_tagCloudPlacerConfig)
            .Should()
            .BeEquivalentTo(Result.Fail<ITagCloudPlacerConfig>("Put rectangles list can't be null"));
    }
    
    [Test]
    public void ValidateTagCloudFormConfig_NullableNearestToTheCenterPointList_ShouldBeFail()
    {
        _tagCloudPlacerConfig.NearestToTheFieldCenterPoints = null;
        _tagCloudPlacerConfigValidator 
            .Validate(_tagCloudPlacerConfig)
            .Should()
            .BeEquivalentTo(Result.Fail<ITagCloudPlacerConfig>("Nearest to the field center points list can't be null"));
    }
}