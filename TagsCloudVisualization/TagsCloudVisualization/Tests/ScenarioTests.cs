using System;
using System.Collections.Generic;
using System.Drawing;
using Autofac;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using TagsCloudVisualization.Logic;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class ScenarioTests
    {
        private IContainer container;
        private string testTextDirectory;

        [OneTimeSetUp]
        public void InitializeTextsDirectory()
        {
            testTextDirectory = TestContext.CurrentContext.TestDirectory + "\\Tests\\TestTexts\\";
        }

        [SetUp]
        public void InitializeProperties()
        {
            container = EntryPoint.InitializeContainer();
        }

        [Test]
        public void ImageIsCreated_WithCorrectSize_AfterResizingImage()
        {
            var settingsProvider = container.Resolve<IImageSettingsProvider>();
            var newSettings = settingsProvider.ImageSettings;

            newSettings.ImageSize =
                new Size(
                    newSettings.ImageSize.Width * 2,
                    newSettings.ImageSize.Height * 2
                );
            settingsProvider.SetImageSettings(newSettings);
            var visualizer = container.Resolve<IVisualizer>();
            var resultImage = visualizer.VisualizeTextFromFile(testTextDirectory + "animals.txt").GetValueOrThrow();

            resultImage.Size.Should().Be(settingsProvider.ImageSettings.ImageSize);
        }

        [Test]
        public void ImageIsCreated_WithoutBoringWords_WithCustomBoringWordsCollection()
        {
            var boringText = "bathroom\nbedroom\nkitchen";
            var boringWordsProvider = container.Resolve<IBoringWordsProvider>();
            var textParser = container.Resolve<IParser>();

            boringWordsProvider.BoringWords = new HashSet<string>(boringText.Split('\n'));

            textParser.ParseToTokens(boringText).GetValueOrThrow().Should().BeEmpty();
        }

        [TestCase("100tags.txt")]
        [TestCase("1000tags.txt")]
        public void ImageCreation_IsTimePermissible_OnBigAmountOfTags(string fileName)
        {
            var visualizer = container.Resolve<IVisualizer>();
            var millisecondsForEachWord = 5;
            var expectedTime = TextRetriever
                                   .RetrieveTextFromFile(testTextDirectory + fileName)
                                   .GetValueOrThrow()
                                   .Split('\n')
                                   .Length
                               * millisecondsForEachWord;

            Action action = () => visualizer.VisualizeTextFromFile(testTextDirectory + fileName);

            action.ExecutionTime().Should().BeLessThan(expectedTime.Milliseconds());
        }
    }
}