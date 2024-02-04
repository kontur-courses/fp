using System.Drawing;
using System.Runtime.CompilerServices;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
[UseReporter(typeof(DiffReporter))]
public class LayoutDrawer_Should
{
    private IPalette palette;
    private IPointGenerator pointGenerator;
    private IDullWordChecker dullWordChecker;
    private IInterestingWordsParser interestingWordsParser;
    private IRectangleLayouter rectangleLayouter;
    private TagLayoutSettings tagLayoutSettings;

    [SetUp]
    public void SetUp()
    {
        palette = new Palette(new[] { Color.Aqua }, Color.Black);
        pointGenerator = new SpiralPointGenerator();
        tagLayoutSettings = new TagLayoutSettings(Algorithm.Spiral, new HashSet<string> { "S", "CONJ" },
            @"TextFiles\ExcludedWords.txt");
        dullWordChecker = new MystemDullWordChecker(tagLayoutSettings);
        interestingWordsParser = new MystemWordsParser(dullWordChecker);
        rectangleLayouter = new RectangleLayouter(tagLayoutSettings, new[] { pointGenerator });
    }

    [TestCase(@"C:\Absolute\Non\Existing\Path", TestName = "Absolute path")]
    [TestCase("NonExistingPath", TestName = "Relative path")]
    [Test]
    public void Fail_WhenFileDoesntExist(string inputFilePath)
    {
        var layoutDrawer = new LayoutDrawer(interestingWordsParser, rectangleLayouter, palette, new Font("Arial", 24));
        var result = layoutDrawer.CreateLayoutImageFromFile(inputFilePath, new Size(1000, 1000), 1);
        using (ApprovalResults.ForScenario(TestContext.CurrentContext.Test.Name))
        {
            result.IsSuccess.Should().BeFalse();
            Approvals.Verify(result.Error);
        }
    }
    
    [Test]
    public void Fail_WhenTagCloudIsTooBig()
    {
        var layoutDrawer = new LayoutDrawer(interestingWordsParser, rectangleLayouter, palette, new Font("Arial", 100));
        var result = layoutDrawer.CreateLayoutImageFromFile("TextFiles/RuWords.txt", new Size(100, 100), 1);
        result.IsSuccess.Should().BeFalse();
        Approvals.Verify(result.Error);
    }
}