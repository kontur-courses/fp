using NUnit.Framework;
using FluentAssertions;
using TagsCloudGenerator.WordsParsers;
using TagsCloudGenerator.WordsConverters;
using TagsCloudGenerator.WordsFilters;
using TagsCloudGeneratorExtensions;
using FailuresProcessing;

namespace TagsCloudGenerator_Tests
{
    internal class ParserConverterFilter_Tests
    {
        private SingletonScopeInstancesContainer container;

        [OneTimeSetUp]
        public void OneTimeSetUp() => container = new SingletonScopeInstancesContainer();

        [TearDown]
        public void TearDown() => container.Get<Settings>().Reset().IsSuccess.Should().BeTrue();

        [TestCase(
            "TestData.txt",
            "ноутбук", "конверт", "хлеб", "тарелка", "тарелки", "клавиатура", "пробел")]
        public void UTF8LinesParserToLowerConverterBoringWordsFilter_Test(
            string pathToRead,
            params string[] expected)
        {
            var linesParser = container.Get<UTF8LinesParser>();
            var toLowerConverter = container.Get<WordsToLowerConverter>();
            var boringWordsFilter = container.Get<BoringWordsFilter>();

            var actual =
                linesParser.ParseFromFile(Metadata.WorkingDirectory + pathToRead)
                .Then(words => toLowerConverter.Execute(words))
                .Then(wordsToLower => boringWordsFilter.Execute(wordsToLower));

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase(
            "TestData.docx",
            "ноутбук", "конверт", "хлеб", "тарелка", "тарелка", "клавиатура", "пробел")]
        public void DocxLinesParserToLowerAndInitialWordFormConvertersBoringWordsFilter_Test(
            string pathToRead,
            params string[] expected)
        {
            var docxLinesParser = container.Get<DocxLinesParser>();
            var toLowerConverter = container.Get<WordsToLowerConverter>();
            var initialWordFormConverter = container.Get<InitialWordsFormConverter>();
            var boringWordsFilter = container.Get<BoringWordsFilter>();

            var actual =
                docxLinesParser.ParseFromFile(Metadata.WorkingDirectory + pathToRead)
                .Then(words => toLowerConverter.Execute(words))
                .Then(toLower => initialWordFormConverter.Execute(toLower))
                .Then(toInitialWordForm => boringWordsFilter.Execute(toInitialWordForm));

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase(
            "TestDataWithVerbs.txt",
            "делать", "бежать", "указать")]
        public void UTF8LinesParserToLowerConverterBoringWordsAndTakenVerbFilter(
            string pathToRead,
            params string[] expected)
        {
            var linesParser = container.Get<UTF8LinesParser>();
            var toLowerConverter = container.Get<WordsToLowerConverter>();
            var boringWordsFilter = container.Get<BoringWordsFilter>();
            var takenVerbsFilter = container.Get<TakenPartsOfSpeechFilter>();
            container.Get<Settings>().TakenPartsOfSpeech = new[] { "v" };

            var actual =
                linesParser.ParseFromFile(Metadata.WorkingDirectory + pathToRead)
                .Then(words => toLowerConverter.Execute(words))
                .Then(toLower => boringWordsFilter.Execute(toLower))
                .Then(noBoringWords => takenVerbsFilter.Execute(noBoringWords));

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().BeEquivalentTo(expected);
        }
    }
}