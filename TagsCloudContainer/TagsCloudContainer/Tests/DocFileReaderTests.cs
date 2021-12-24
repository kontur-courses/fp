using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.TextPreparation;

namespace TagsCloudContainer.Tests
{
    public class DocFileReaderTests
    {
        [Test]
        public void GetAllWords_Fails_WhenPathIsNull()
        {
            new DocFileReader(new WordsReader()).GetAllWords(null).Error.Should().Be("Path can't be null");
        }

        [Test]
        public void GetAllWords_CallFileReader_WhenCanReadFile()
        {
            var fake = A.Fake<IWordsReader>();
            A.CallTo(() => fake.ReadAllWords(null)).WithAnyArguments().Returns(new Result<List<string>>());

            new DocFileReader(fake).GetAllWords("../../input.docx");

            A.CallTo(() => fake.ReadAllWords(null)).WithAnyArguments().MustHaveHappened(1, Times.Exactly);
        }

        [Test]
        public void GetAllWords_Fails_WhenCantReadFile()
        {
            Action act = () => new DocFileReader(new WordsReader()).GetAllWords("../../input.txt");

            act.Should().Throw<Exception>();
        }
    }
}