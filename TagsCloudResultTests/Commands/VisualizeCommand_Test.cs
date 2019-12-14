using System;
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
    public class VisualizeCommand_Test
    {
        private VisualizeCommand command;

        [SetUp]
        public void SetUp()
        {
            var settings = new SettingsManager();
            var parser = new CloudWordsParser(() => settings.GetWordsParserSettings());
            var layouter = new CloudLayouter(() => settings.GetLayouterSettings());
            var visualizer = new CloudVisualizer(() => settings.GetVisualizerSettings());
            var saver = new ImageSaver(() => settings.GetImageSaverSettings());
            var cloud = new TagsCloud(parser, layouter, visualizer, saver);
            command = new VisualizeCommand(cloud, settings);
        }

        [Test]
        public void Act_Should_ThrowArgumentException_When_WrongArgumentsCount()
        {
            Following.Code(() => command.Act(new[] {"def", "1280", "720", "black", "red"}))
                .Should().Throw<ArgumentException>();
        }

        [TestCase("0,5", TestName = "when too small")]
        [TestCase("0", TestName = "when zero")]
        [TestCase("-1", TestName = "when negative")]
        [TestCase("ab", TestName = "when not a number")]
        public void Act_Should_ThrowArgumentException_When_IncorrectWidthValue(string width)
        {
            var args = new[] {"def", width, "720", "black", "red", "blue", "false", "Arial"};
            Following.Code(() => command.Act(args)).Should().Throw<ArgumentException>();
        }

        [TestCase("0,5", TestName = "when too small")]
        [TestCase("0", TestName = "when zero")]
        [TestCase("-1", TestName = "when negative")]
        [TestCase("ab", TestName = "when not a number")]
        public void Act_Should_ThrowArgumentException_When_IncorrectHeightValue(string height)
        {
            var args = new[] {"def", "1280", height, "black", "red", "blue", "false", "Arial"};
            Following.Code(() => command.Act(args)).Should().Throw<ArgumentException>();
        }

        [Test()]
        public void Act_Should_ThrowArgumentException_When_IncorrectBitmapMaker()
        {
            var args = new[] {"idontexist", "1280", "720", "black", "red", "blue", "false", "Arial"};
            Following.Code(() => command.Act(args)).Should().Throw<ArgumentException>();
        }
    }
}