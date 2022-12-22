using FluentAssertions;
using NUnit.Framework;
using TagCloud.BoringWordsRepositories;
using TagCloud.Readers;

namespace TagCloudTests
{
    public class TextFileBoringWordsStorageTests
    {
        private IBoringWordsStorage boringWordsStorage;
        private IBoringWordsReader wordReader;
        [SetUp]
        public void CreateTextFileBoringWordsStorageTests()
        {
            wordReader = new SingleWordInRowTextFileReader();
            boringWordsStorage = new TextFileBoringWordsStorage(wordReader);
        }

        [Test]
        public void TextFileBoringWordsStorage_GetBoringWords_ShouldReturnedEmptyCollectionAfterCtor()
        {
            boringWordsStorage.GetBoringWords().GetValueOrThrow().Should().BeEmpty();
        }

        [TestCase("empty.txt", TestName = "file without words")]
        public void TextFileBoringWordsStorage_GetBoringWords_ShouldReturnedEmptyCollectionWhen(string path)
        {
            boringWordsStorage.LoadBoringWords(path);
            boringWordsStorage.GetBoringWords().GetValueOrThrow().Should().BeEmpty();
        }

        [TestCase("BoringWordsDictionary.txt", TestName = "file with words")]
        public void TextFileBoringWordsStorage_GetBoringWords_ShouldReturnedCollectionWhen(string path)
        {
            boringWordsStorage.LoadBoringWords(path);
            boringWordsStorage.GetBoringWords().GetValueOrThrow().Should().HaveCountGreaterThan(1);
        }

        [Test]
        public void TextFileBoringWordsStorage_FileExtFilter_IsEqualToBaseIBoringWordsReader()
        {
            boringWordsStorage.FileExtFilter.Should().BeEquivalentTo(wordReader.FileExtFilter);
        }
    }
}
