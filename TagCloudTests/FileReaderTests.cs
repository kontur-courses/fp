using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.file_readers;

namespace TagCloudTests
{
    [TestFixture]
    public class FileReaderTests
    {
        private FileReader reader;
        private string filename;
        private const string Dirname = "TxtReaderInput";

        [SetUp]
        public void SetUp()
        {
            reader = new FileReader();

            if (!Directory.Exists(Dirname))
                Directory.CreateDirectory(Dirname);
            filename = $"{Environment.CurrentDirectory}\\{Dirname}\\input.txt";
        }

        [Test]
        public void GetWords_ThrowNullArgumentException_IfFilenameIsNull()
        {
            reader.GetWords(null).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetWords_ThrowArgumentException_IfFileContainsSomeWordsInOneLine()
        {
            File.WriteAllText(filename, "word1 word2");
            reader.GetWords(filename).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void GetWords_ReturnsAllWordsFromFile()
        {
            File.WriteAllText(filename, "word1\nword2\nword3");
            reader.GetWords(filename)
                .GetValueOrThrow()
                .Should()
                .Contain("word1")
                .And.Contain("word2")
                .And.Contain("word3");
        }

        [Test]
        public void GetWords_ReturnsEmptySequence()
        {
            File.WriteAllText(filename, "");
            reader.GetWords(filename)
                .GetValueOrThrow()
                .Should()
                .BeEmpty();
        }
    }
}