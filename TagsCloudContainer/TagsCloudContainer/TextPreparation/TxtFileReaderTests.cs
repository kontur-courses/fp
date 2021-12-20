using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer.TextPreparation
{
    public class TxtFileReaderTests
    {
        [Test]
        public void GetAllWords_Fails_WhenPathIsNull()
        {
            new TxtFileReader(new WordsReader()).GetAllWords(null).Error.Should().Be("Path can't be null");
        }

        [Test]
        public void GetAllWords_CallFileReader_WhenCanReadFile()
        {
            var fake = A.Fake<IWordsReader>();
            A.CallTo(() => fake.ReadAllWords(null)).WithAnyArguments().Returns(new Result<List<string>>());

            new TxtFileReader(fake).GetAllWords("../../input.txt");

            A.CallTo(() => fake.ReadAllWords(null)).WithAnyArguments().MustHaveHappened(1, Times.Exactly);
        }
    }
}