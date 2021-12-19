using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.FileReaders;

namespace TagsCloudContainerTests
{
    public class DocFileReader_Tests
    {
        private readonly DocFileReader docFileReader = new DocFileReader();
        private const string FileName = "../../../DocTestFile.docx";
        
        [Test]
        public void ReadWordsFromFile_ReturnsFailResult_WhenFileDoesNotExist()
        {
            var result = docFileReader.ReadWordsFromFile("fakeFile");
            
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("File doesn't exist fakeFile");
        }
        
        [Test]
        public void ReadWordsFromFile_WorksCorrectlyWithDoc()
        {
            var expectedWords = new[] { "firstWord", "secondWord", "thirdWord" };
            
            var words = docFileReader.ReadWordsFromFile(FileName).GetValueOrThrow();

            words.Should().BeEquivalentTo(expectedWords);
        }
    }
}