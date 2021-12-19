using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.FileReaders;
using TagsCloudContainer.Settings;
using TagsCloudContainer.WordFilters;

namespace TagsCloudContainerTests
{
    public class BoringWordsFilter_Tests
    {
        private BoringWordsFilter filter;
        private FileReaderFactory factory;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            factory = new FileReaderFactory(new[] { new TxtFileReader() });
            const string boringWordsPath = "BoringWords.txt";
            var settings = new AppSettings()
            {
                BoringWordsPath = boringWordsPath
            };

            filter = new BoringWordsFilter(settings, factory);
            var boringWords = new[] { "they", "we", "are" };
            File.WriteAllText(boringWordsPath, string.Join("\n", boringWords));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            File.Delete("BoringWords.txt");
        }

        [Test]
        public void Filter_FiltersWordsCorrectly()
        {
            var words = new[] { "they", "me", "he", "she", "was", "are", "we" };
            var expectedFilteredWords = new[] { "me", "he", "she", "was" };

            var filteredWords = filter.Filter(words).GetValueOrThrow();

            filteredWords.Should().BeEquivalentTo(expectedFilteredWords);
        }
        
        [Test]
        public void Filter_ReturnsFailResult_WhenFileDoesNotExist()
        {
            var settings = new AppSettings()
            {
                BoringWordsPath = "fakeFile.txt"
            };
            var filter = new BoringWordsFilter(settings, factory);

            var result = filter.Filter(new[] { "some words" });

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("File doesn't exist fakeFile.txt");
        }
    }
}