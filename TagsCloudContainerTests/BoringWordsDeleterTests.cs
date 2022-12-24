using CommandLine;
using FluentAssertions;
using NUnit.Framework;
using TagCloudContainer;
using TagCloudContainer.UI;

namespace TagsCloudContainerTests
{
    public class BoringWordsDeleterTests
    {
        private IUi parsedArguments;
        [SetUp]
        public void SetUp()
        {
            var app = new ConsoleUiSettings();
            parsedArguments = Parser.Default.ParseArguments<ConsoleUiSettings>(new string[] { }).Value;
        }
        [Test]
        public void BoringWordsDeleter_ShouldNotDeleteNotBoringWords()
        {
            var notBoringWords = new[] {"котик", "котенок", "кошка", "кисуля", "котяра"};
            var result = BoringWordsDeleter.DeleteBoringWords(notBoringWords).Value;
            result.Should().BeEquivalentTo(notBoringWords);
        }

        [Test]
        public void BoringWordsDeleter_ShouldDeleteBoringWords()
        {
            var notBoringWords = new[] {"кто", "на", "где", "а", "и"};
            var result = BoringWordsDeleter.DeleteBoringWords(notBoringWords);
            result.Value.Should().BeEmpty();
        }

        [Test]
        public void BoringWordsDeleter_ShouldDeleteEmptyStrings()
        {
            var notBoringWords = new[] {"а", "и", "не", "", "спатеньки"};
            var result = BoringWordsDeleter.DeleteBoringWords(notBoringWords).Value;
            result.Should().BeEquivalentTo("спатеньки");
        }
    }
}