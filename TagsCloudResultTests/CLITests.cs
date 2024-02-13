using FluentAssertions;
using TagsCloudResult.UI;

namespace TagsCloudResultTests;

[TestFixture]
public class CLITests
{
    [Test]
    public void MissRequiredCommand_Should_Throw()
    {
        ApplicationArguments.Setup([
            """
            -i="/Users/draginsky/Rider/fp/TagsCloudResult/src/words.txt"
            -o="/Users/draginsky/Rider/fp/TagsCloudResult/out/res"
            """
        ]).IsErr.Should().BeTrue();
    }
    
    [Test]
    public void WrongFontPath_Should_Throw()
    {
        ApplicationArguments.Setup([
            """
            -i="/Users/draginsky/Rider/fp/TagsCloudResult/src/words.txt"
            -o="/Users/draginsky/Rider/fp/TagsCloudResult/out/res"
            --fontpath="wrong"
            """
        ]).IsErr.Should().BeTrue();
    }
}