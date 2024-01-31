using FluentAssertions;
using TagsCloudResult.UI;

namespace TagsCloudResultTests;

[TestFixture]
public class CLITests
{
    [Test]
    public void HelpCommand_Should_NotThrow()
    {
        var action = () => ApplicationArguments.Setup(["--help"]);
        action.Should().NotThrow();
    }

    [Test]
    public void MissRequiredCommand_Should_Throw()
    {
        var action = () => ApplicationArguments.Setup([
            """
            -i="/Users/draginsky/Rider/fp/TagsCloudResult/src/words.txt"
            -o="/Users/draginsky/Rider/fp/TagsCloudResult/out/res"
            """
        ]);
        action.Should().Throw<ArgumentNullException>();
    }
}