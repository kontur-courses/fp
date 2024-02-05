using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Algorithm;
using TagsCloudContainer.Infrastucture;

namespace TagsCloudContainerTests
{
    public class FileParser_Should
    {
        private FileParser parser;

        [SetUp]
        public void SetUp() 
        {  
            parser = new FileParser();
        }

        [Test]
        public void ReturnsResultFail_IfMoreThanOneWordInLine()
        {
            var incorrectDataFilePath = GetProjectDirectory() + @"\src\incorrectData.txt";
            var words = parser.ReadWordsInFile(incorrectDataFilePath);

            words.IsSuccess.Should().BeFalse();
            words.Error.Should().Be($"The file with the path {incorrectDataFilePath} has incorrect content:" +
                        $" more than one word in the line.");
        }

        [Test]
        public void ReduceWordsToLowercase()
        {
            var expectedList = new List<string> { "привет", "занятие", "яблоко", "домашка" };
            var uppercaseDataFilePath = GetProjectDirectory() + @"\src\upperCase.txt";
            var words = parser.ReadWordsInFile(uppercaseDataFilePath);

            words.IsSuccess.Should().BeTrue();
            words.Value.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void RemoveSpacesFromTheBeginningAndEndOfline()
        {
            var expectedList = new List<string> { "привет", "занятие", "яблоко", "домашка" };
            var uppercaseDataFilePath = GetProjectDirectory() + @"\src\trimData.txt";
            var words = parser.ReadWordsInFile(uppercaseDataFilePath);

            words.IsSuccess.Should().BeTrue();
            words.Value.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void ParseEmptyList_WhenFileIsEmpty()
        {
            var emptyDataFilePath = GetProjectDirectory() + @"\src\emptyData.txt";
            var words = parser.ReadWordsInFile(emptyDataFilePath);

            words.IsSuccess.Should().BeTrue();
            words.Value.Should().BeEmpty();
        }



        [Test]
        public void ReturnResultFail_WhenFailDontExist()
        {
            var path = "";
            var words = parser.ReadWordsInFile(path);

            words.IsSuccess.Should().BeFalse();
            words.Error.Should().Be($"The file with the path {path} was not found");
        }


        public string GetProjectDirectory()
        {
            var binDirectory = AppContext.BaseDirectory;
            var projectDirectory = Directory.GetParent(binDirectory).Parent.Parent.Parent.FullName;

            return projectDirectory;
        }
    }
}
