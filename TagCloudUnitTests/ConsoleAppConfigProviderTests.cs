using FluentAssertions;
using NUnit.Framework;
using TagCloud.AppConfiguration;

namespace TagCloudUnitTests
{
    [TestFixture]
    internal class ConsoleAppConfigProviderTests
    {
        private ConsoleAppConfigProvider provider;

        private string args;

        [SetUp]
        public void SetUp()
        {
            provider = new ConsoleAppConfigProvider();

            args = "-i Text.txt -o Output.png";
        }

        [TestCase(" -z InvalidCloudForm", TestName = "Invalid cloud form")]
        [TestCase(" -w -100 -h 100", TestName = "Image width is negative")]
        [TestCase(" -w 100 -h -100", TestName = "Image height is negative")]
        [TestCase(" -w 0 -h 100", TestName = "Image width is zero")]
        [TestCase(" -w 100 -h 0", TestName = "Image height is zero")]
        [TestCase(" -b InvalidBackgroundColor", TestName = "Invalid background color")]
        [TestCase(" -f InvalidFontFamily", TestName = "Invalid font family")]
        [TestCase(" -l -10 -p 100", TestName = "Min font size is negative")]
        [TestCase(" -l 10 -p -100", TestName = "Max font size is negative")]
        [TestCase(" -l 0 -p 100", TestName = "Min font size is zero")]
        [TestCase(" -l 10 -p 0", TestName = "Max font size is zero")]
        [TestCase(" -l 10 -p 5", TestName = "Min font size is greater than max font size")]
        [TestCase(" -k InvalidWordColoring", TestName = "Invalid word coloring")]
        public void Test(string testingsArgs)
        {
            args += testingsArgs;

            var config = new ConsoleAppConfigProvider().GetAppConfig(args.Split(' '));

            config.IsSuccess.Should().BeFalse();
        }

    }
}
