using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.BoringWordsRepositories;
using TagCloud.Readers;
using TagCloud.WordPreprocessors;

namespace TagCloudTests
{
    public class SimpleWordPreprocessorTests
    {
        private IReader wordsReader;
        private IBoringWordsStorage boringWordsStorage;
        private IWordPreprocessor wordPreprocessor;
        private readonly string boringWordsPath = @"BoringWordsRepositories\BoringWordsDictionary.txt";

        [SetUp]
        public void CreateWords()
        {
            wordsReader = new SingleWordInRowTextFileReader();
            boringWordsStorage = new TextFileBoringWordsStorage(new SingleWordInRowTextFileReader());
            wordsReader.SetFile("aboutKonturWords.txt");
        }

        [TestCase(null,null, 
            "Error when reading boring words: path is not defined. Error when reading words: path is not defined", TestName = "null words paths")]
        [TestCase("notExistedFile.txt", "notExistedBoringWordsFile.txt",
            "Error when reading boring words: file notExistedBoringWordsFile.txt not found. Error when reading words: file notExistedFile.txt not found", TestName = "not existed words paths")]
        [TestCase(null, "notExistedBoringWordsFile.txt", 
            "Error when reading boring words: file notExistedBoringWordsFile.txt not found. Error when reading words: path is not defined", TestName = "not existed and null words paths")]
        //[TestCase("aboutKonturWords.txt", null, "Error when reading boring words: path is not defined", TestName = "null boring words path")]
        public void SimpleWordPreprocessor_GetPreprocessedWords_ShouldReturnErrorWhen(string wordPath, string boringWordsPath, string errorText)
        {
            wordsReader.SetFile(wordPath);
            boringWordsStorage.LoadBoringWords(boringWordsPath);
            var preprocessedWords = GetPreprocessedWords();

            preprocessedWords.IsSuccess.Should().BeFalse();
            preprocessedWords.Error.Should().Be(errorText);
        }

        [Test]
        public void SimpleWordPreprocessor_GetPreprocessedWords_ShouldReturnСonvertWordsToLowerCase()
        {
            var words = wordsReader.ReadWords();
            var preprocessedWords = GetPreprocessedWords();

            words.GetValueOrThrow().Should().Contain(word => word.Any(c => char.IsUpper(c)));
            preprocessedWords.GetValueOrThrow().Should().NotContain(word => word.Any(c => char.IsUpper(c)));
        }

        [Test]
        public void SimpleWordPreprocessor_GetPreprocessedWords_ShouldReturnRemoveBoringWordsFromWords()
        {
            var words = wordsReader.ReadWords();
            boringWordsStorage.LoadBoringWords(boringWordsPath);

            var boringWords = boringWordsStorage.GetBoringWords().GetValueOrThrow();
            var preprocessedWords = GetPreprocessedWords();

            words.GetValueOrThrow().Should().Contain(word => boringWords.Contains(word));
            preprocessedWords.GetValueOrThrow().Should().NotContain(word => boringWords.Contains(word));
        }

        private Result<IEnumerable<string>> GetPreprocessedWords()
        {
            wordPreprocessor = new SimpleWordPreprocessor(wordsReader, boringWordsStorage);
            var preprocessedWords = wordPreprocessor.GetPreprocessedWords();
            return preprocessedWords;
        }
    }
}
