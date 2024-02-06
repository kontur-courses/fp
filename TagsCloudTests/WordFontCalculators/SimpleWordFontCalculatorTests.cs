using System.Drawing;
using FluentAssertions;
using TagsCloud.ConsoleCommands;
using TagsCloud.WordFontCalculators;

namespace TagsCloudTests.WordFontCalculators;

[TestFixture]
public class SimpleWordFontCalculatorTests
{
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
        var options = new Options() { TagsFont = "Arial" };
        var fontCalculator = new SimpleWordFontCalculator(options);
        var result = fontCalculator.GetWordsFont(dict).GetValueOrThrow();
            result.Should().BeEquivalentTo(dictOutput);
    }
}