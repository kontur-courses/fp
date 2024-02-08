using FluentAssertions;
using NUnit.Framework;
using StopWord;
using TagsCloudContainer.Algorithm;

namespace TagsCloudContainerTests
{
    public class WordProcessor_Should
    {
        private WordProcessor wordProcessor;
        private string sourceFilePath;
        private string boringFilePath;
        private string[]? stopWords = StopWords.GetStopWords("ru");

        [SetUp]
        public void SetUp()
        {
            wordProcessor = new WordProcessor(new FileParser());
            sourceFilePath = GetProjectDirectory();
            boringFilePath = GetProjectDirectory();
        }

        [Test]
        public void GetInterestingWords_WhenFileWithBoringWordsIsEmpty()
        {
            sourceFilePath += @"\src\sourceData.txt";
            boringFilePath += @"\src\emptyData.txt";

            var interestingWords = wordProcessor.GetInterestingWords(sourceFilePath, boringFilePath);

            interestingWords.IsSuccess.Should().BeTrue();
            interestingWords.Value.Should().NotContain(stopWords);
        }

        [Test]
        public void GetInterestingWords_WhenFileWithBoringWordsIsNotEmpty()
        {
            sourceFilePath += @"\src\sourceData.txt";
            boringFilePath += @"\src\boringData.txt";

            var interestingWords = wordProcessor.GetInterestingWords(sourceFilePath, boringFilePath);
            var boringWords = new FileParser().ReadWordsInFile(boringFilePath);

            interestingWords.IsSuccess.Should().BeTrue();
            interestingWords.Value.Should().NotContain(stopWords);
            interestingWords.Value.Should().NotContain(boringWords.Value);
        }

        [Test]
        public void CorrectlyCalculatesFrequencyWords()
        {
            var expectedDictonary = new Dictionary<string, int>
            {
                { "js", 3 },
                { "python", 4},
                { "c#", 5 },
                { "c++", 2 },
                { "rust", 2 },
                { "go", 4 },
                { "1c", 1 }
            };
            sourceFilePath += @"\src\sourceData.txt";
            boringFilePath += @"\src\emptyData.txt";

            var wordsFrequency = wordProcessor.CalculateFrequencyInterestingWords(sourceFilePath, boringFilePath);

            wordsFrequency.IsSuccess.Should().BeTrue();
            wordsFrequency.Value.Should().Contain(expectedDictonary);
        }

        [Test]
        public void ReturnEmptyDictionary_WhenSourceWordFileIsEmpty()
        {
            sourceFilePath += @"\src\emptyData.txt";
            boringFilePath += @"\src\emptyData.txt";

            var wordsFrequency = wordProcessor.CalculateFrequencyInterestingWords(sourceFilePath, boringFilePath);

            wordsFrequency.IsSuccess.Should().BeTrue();
            wordsFrequency.Value.Should().BeEmpty();
        }

        [TestCase(" ", @"\src\boringData.txt", 
            TestName = "The file with the words for the tag cloud don't exist")]
        [TestCase(@"\src\sourceData.txt", " ",
            TestName = "The file with boring words don't exist")]
        [TestCase(" ", " ", 
            TestName = "Files with boring words and tag cloud words don't exist")]
        public void ReturnResultFail_WhenFilesDontExist(string sourceFileName, string boringFileName)
        {
            sourceFilePath += sourceFileName;
            boringFilePath += boringFileName;

            var wordsFrequency = wordProcessor.CalculateFrequencyInterestingWords(sourceFilePath, boringFilePath);

            wordsFrequency.IsSuccess.Should().BeFalse();
        }

        public string GetProjectDirectory()
        {
            var binDirectory = AppContext.BaseDirectory;
            var projectDirectory = Directory.GetParent(binDirectory).Parent.Parent.Parent.FullName;

            return projectDirectory;
        }
    }
}
