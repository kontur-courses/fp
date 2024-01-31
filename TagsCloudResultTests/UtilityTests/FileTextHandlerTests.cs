using FluentAssertions;
using TagsCloudResult.Utility;

namespace TagsCloudResultTests.UtilityTests;

[TestFixture]
public class FileTextHandlerTests
{
    [TestCase("src/mars.docx", 
        "A new study published in the journal Science shows definitive evidence of organic " +
        "matter on the surface of Mars. The data was collected by NASA's nuclear-powered rover " +
        "Curiosity. It confirms earlier findings that the Red Planet once contained carbon-based " +
        "compounds. These compounds – also called organic molecules – are essential ingredients " +
        "for life as scientists understand it.", TestName = "InDocxFormat")]
    [TestCase("src/mars.doc", 
        "A new study published in the journal Science shows definitive evidence of organic " +
        "matter on the surface of Mars. The data was collected by NASA's nuclear-powered rover " +
        "Curiosity. It confirms earlier findings that the Red Planet once contained carbon-based " +
        "compounds. These compounds – also called organic molecules – are essential ingredients " +
        "for life as scientists understand it.", TestName = "InDocFormat")]
    [TestCase("src/boringWords.txt", "One, Two, Three, Two, Three, Three", TestName = "InTxtFormat")]
    public void ReadText_Should_CorrectReadAllText_(string input, string expected)
    {
        var actual = new FileTextHandler().ReadText(Utility.GetAbsoluteFilePath(input)).Unwrap();
        
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void ReadText_Should_ReturnFailOnWrongFilepath()
    {
        var actual = new FileTextHandler().ReadText(Utility.GetAbsoluteFilePath("wrong"));
        
        actual.IsErr.Should().BeTrue();
    }
    
    [Test]
    public void ReadText_Should_ReturnFailOnInaccessibleFile()
    {
        var path = Utility.GetAbsoluteFilePath("words.txt");
        var handle = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        
        var actual = new FileTextHandler().ReadText(path);
        
        actual.IsErr.Should().BeTrue();
        
        handle.Close();
    }
}