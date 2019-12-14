using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudResult;
using TagsCloudResult.ApplicationRunning;
using TagsCloudResult.ApplicationRunning.Commands;
using TagsCloudResult.CloudLayouters;
using TagsCloudResult.CloudVisualizers;
using TagsCloudResult.CloudVisualizers.ImageSaving;
using TagsCloudResult.TextParsing.CloudParsing;

namespace TagsCloudResultTests.Commands
{
    [TestFixture]
    public class SaveCommand_Test
    {
        [SetUp]
        public void SetUp()
        {
            var settings = new SettingsManager();
            var parser = new CloudWordsParser(() => settings.GetWordsParserSettings());
            var layouter = new CloudLayouter(() => settings.GetLayouterSettings());
            var visualizer = new CloudVisualizer(() => settings.GetVisualizerSettings());
            var saver = new ImageSaver(() => settings.GetImageSaverSettings());
            var cloud = new TagsCloud(parser, layouter, visualizer, saver);
            command = new SaveCommand(cloud, settings);
        }

        private SaveCommand command;

        [Test]
        public void Act_Should_ThrowArgumentException_When_IncorrectFormat()
        {
            var path = Directory.GetCurrentDirectory();
            command.ParseArguments(new[] {path, "maybeonedayidk"}).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void Act_Should_ThrowArgumentException_When_IncorrectPath()
        {
            command.ParseArguments(new[] {"toocooltobetrue", "coolimage"}).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void Act_Should_ThrowArgumentException_When_WrongArgumentsCount()
        {
            command.ParseArguments(new[] {@"D:\coolpath", "coolimage"}).IsSuccess.Should().BeFalse();
        }
    }
}