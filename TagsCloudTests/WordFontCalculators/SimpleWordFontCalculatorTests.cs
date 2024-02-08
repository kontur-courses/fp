using System.Drawing;
using FluentAssertions;
using TagsCloud.Options;
using TagsCloud.WordFontCalculators;

namespace TagsCloudTests.WordFontCalculators;

[TestFixture]
public class SimpleWordFontCalculatorTests
{

    private SimpleWordFontCalculator simpleWordFontCalculator;
    
    [SetUp]
    public void SetUp()
    {
        var options = new LayouterOptions() { FontName = "Arial" };
        simpleWordFontCalculator = new SimpleWordFontCalculator(options);
    }
    [Test]
    public void SimpleWordFontCalculator_ShouldReturnFontSizeAsWordCount()
    {
        var dict = new Dictionary<string, int>()
        {
            { "apple", 10 },
            { "banana", 5 },
            { "orange", 8 },
            { "grape", 12 }
        };
        
        var dictOutput = new Dictionary<string, Font>()
        {
            { "apple", new Font("Arial",10)},
            { "banana",  new Font("Arial",5) },
            { "orange",  new Font("Arial",8) },
            { "grape",  new Font("Arial",12) }
        };
       
        simpleWordFontCalculator.GetWordsFont(dict).GetValueOrThrow().Should().BeEquivalentTo(dictOutput);
    }

    [Test]
    public void SimpleWordFontCalculator_ShouldReturnErrorResult_WhenInputDictionaryContainsNoElements()
    {
        simpleWordFontCalculator.GetWordsFont(new Dictionary<string, int>() { }).IsSuccess.Should().BeFalse();
    }
    
    [Test]
    public void SimpleWordFontCalculator_ShouldReturnErrorResult_WhenInputDictionaryContainsValueEqualZero()
    {
        simpleWordFontCalculator.GetWordsFont(new Dictionary<string, int>() { {"apple",0}}).IsSuccess.Should().BeFalse();
    }
    
}