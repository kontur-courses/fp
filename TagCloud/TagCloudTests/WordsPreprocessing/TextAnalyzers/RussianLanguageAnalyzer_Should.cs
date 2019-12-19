using FluentAssertions;
using NUnit.Framework;
using TagCloud.WordsPreprocessing.TextAnalyzers;


namespace TagCloudTests.WordsPreprocessing.TextAnalyzers
{
    public class RussianLanguageAnalyzer_Should
    {
        private RussianLanguageAnalyzer analyzer;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            analyzer = TagCloud.Program.InitializeContainer().Get<RussianLanguageAnalyzer>();
        }

        [Test]
        public void GetWords_ReturnsWords()
        {
            var wordsResult = analyzer.GetWords(new[] {"Привет", "Пока"}, 2);
            if (wordsResult.IsSuccess ||
                wordsResult.Error != "Can not include external library. Please select another analyzer")
                wordsResult.GetValueOrThrow().Should().HaveCount(2);
        }

        [Test]
        public void GetWords_ReturnsAllWordsIfLessThanRequest()
        {
            var wordsResult = analyzer.GetWords(new[] {"Привет", "Пока", "Отдать"}, 5);
            if (wordsResult.IsSuccess ||
                wordsResult.Error != "Can not include external library. Please select another analyzer")
                wordsResult.GetValueOrThrow().Should().HaveCount(3);
        }

        [Test]
        public void GetWords_ReturnsInitialWordForm()
        {
            var wordsResult = analyzer.GetWords(new[] {"Сделал"}, 1);
            if (wordsResult.IsSuccess ||
                wordsResult.Error != "Can not include external library. Please select another analyzer")
                wordsResult.Value[0].Value.Should().Be("сделать");
        }

        [Test]
        public void GetWords_ReturnWordWithRightFrequencyAndInRightOrder()
        {
            var wordsResult = analyzer.GetWords(new[] {"Привет", "Привет", "Сделать"}, 2);
            if (!wordsResult.IsSuccess &&
                wordsResult.Error == "Can not include external library. Please select another analyzer") return;

            var words = wordsResult.GetValueOrThrow();
            words[0].Value.Should().Be("привет");
            words[0].Frequency.Should().BeApproximately(0.66, 1e-2);
            words[1].Value.Should().Be("сделать");
            words[1].Frequency.Should().BeApproximately(0.33, 1e-2);
        }
    }
}