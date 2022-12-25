using System;
using FluentAssertions;
using NUnit.Framework;
using TagCloudContainer.UI;
using TagCloudContainer.WordsColoringAlgorithms;

namespace TagsCloudContainerTests
{
    public class WordsColoringFactoryTests
    {
        private IUi settings;

        [SetUp]
        public void SetUp()
        {
            settings = new ConsoleUiSettings();
        }

        [TestCase("d", typeof(DefaultWordsPainter), TestName = "Default painter")]
        [TestCase("gd", typeof(GradientDependsOnSizePainter), TestName = "Gradient depends on size painter")]
        [TestCase("g", typeof(GradientWordsPainter), TestName = "Gradient painter")]
        public void ColoringFactory_ShouldReturnCorrectValue_OnCorrectInput(string algorithmName,
            Type expectedResult)
        {
            settings.WordsColoringAlgorithm = algorithmName;
            var painter = new WordsColoringFactory(() => settings).Create();
            painter.Value.GetType().Should().Be(expectedResult);
        }
    }
}