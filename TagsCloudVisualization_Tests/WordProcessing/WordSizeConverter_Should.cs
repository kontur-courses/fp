using System.Windows.Forms;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.WordsProcessing;

namespace TagsCloudVisualization_Tests.WordProcessing
{

    public class WordSizeConverter_Should
    {
        private FontSettings fontSettings;
        private WordSizeConverter converter;

        [SetUp]
        public void SetUp()
        {
            fontSettings = new FontSettings("Times New Roman", 0);
            converter = new WordSizeConverter(fontSettings);
        }

        [Test]
        public void ConvertWeightedWordsCorrectly()
        {
            var words = new[] { new WeightedWord("a", 3)};
            var font = fontSettings.GetFont(3, 3, 3).GetValueOrThrow();
            var expected = new[] {new SizedWord("a", font, TextRenderer.MeasureText("a", font))};
            converter.Convert(words).GetValueOrThrow().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ConvertWeighted_WhenFontSettingsAreWrong_Fail()
        {
            var words = new[] { new WeightedWord("b", 3), new WeightedWord("a", 3)};
            converter = new WordSizeConverter(new FontSettings("D", 0));
            converter.Convert(words).Error.Should().StartWith("Unable download font settings");
        }

        [Test]
        public void ConvertWeighted_WhenEmptyWord_Fail()
        {
            var words = new[] { new WeightedWord("", 3), new WeightedWord("a", 3) };
            converter.Convert(words).Error.Should().EndWith("grater than zero");
        }

        [Test]
        public void ConvertWeighted_WhenFontSettingsWrongAdnEmptyWord_FailOnFirst()
        {
            var words = new[] { new WeightedWord("", 3), new WeightedWord("a", 3) };
            converter = new WordSizeConverter(new FontSettings("D", 0));
            converter.Convert(words).Error.Should().StartWith("Unable download font settings");
        }
    }
}
