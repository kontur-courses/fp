using System;
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
        private SaveCommand command;

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

        [Test]
        public void Act_Should_ThrowArgumentException_When_WrongArgumentsCount()
        {
            Following.Code(() => command.Act(new[] {@"D:\coolpath", "coolimage"})).Should().Throw<ArgumentException>();
        }

        [Test]
        public void Act_Should_ThrowArgumentException_When_IncorrectPath()
        {
            Following.Code(() => command.Act(new[] {"toocooltobetrue", "coolimage"})).Should().Throw<ArgumentException>();
        }

        [Test]
        public void Act_Should_ThrowArgumentException_When_IncorrectFormat()
        {
            var path = Directory.GetCurrentDirectory();
            Following.Code(() => command.Act(new[] {path, "maybeonedayidk"})).Should().Throw<ArgumentException>();
        }
    }
}