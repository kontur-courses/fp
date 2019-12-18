using FluentAssertions;
using NUnit.Framework;
using TagsCloudResult;
using TagsCloudResult.Infrastructure.Common;


namespace TagsCloudResult_Tests
{
    public class ArgumentParserTests
    {
        private readonly ConsoleArgumentParser parser = new ConsoleArgumentParser();

        [Test]
        public void GetFilePath_IfGetFileName()
        {
            var args = new[] {"-f", "file", "-n", "test"};

            var actual = parser.GetSettings(args).Path;
            actual.Should().Be("file");
        }

        [Test]
        public void GetFilePath_IfNotGetRequiredFileName_ReturnNull()
        {
            var args = new[] {"-w", "140", "-h", "180"};

            var actual = parser.GetSettings(args);

            actual.Should().BeNull();
        }


        [Test]
        public void GetWordSettings_IfGetFontName()
        {
            var args = new[] {"-f", "file", "-n", "test", "-o", "Universe"};

            var actual = parser.GetSettings(args).WordsSetting;

            actual.FontName.Should().Be("Universe");
        }

        [Test]
        public void GetWordSettings_IfGetSetBrash()
        {
            var args = new[] {"-f", "file", "-n", "test", "-c", "Red"};

            var actual = parser.GetSettings(args).WordsSetting;

            actual.Color.Should().Be("Red");
        }

        [Test]
        public void GerImageSetting_Width_Height()
        {
            var args = new[] {"-f", "file", "-n", "test", "-w", "180", "-h", "100"};

            var actual = parser.GetSettings(args).ImageSetting;

            actual.Should().BeEquivalentTo(new ImageSetting(100, 180, "White", "png", "test"));
        }

        [Test]
        public void GetAlgorithmSettings()
        {
            var args = new[] {"-f", "file", "-n", "test", "-a", "True"};

            var actual = parser.GetSettings(args).AlgorithmsSettings;
            
            actual.Should().BeEquivalentTo(new AlgorithmsSettings(true));
        }
    }
}