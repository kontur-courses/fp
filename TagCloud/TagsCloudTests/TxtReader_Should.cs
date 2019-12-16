using System;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.FileReader;
using System.IO;
using System.Collections.Generic;
using TagsCloud.PathValidators;

namespace TagsCloudTests
{
    class TxtReader_Should
    {
        private TxtReader txtReader;

        [SetUp]
        public void SetUp()
        {
            var pathValidator = new PathValidator();
            txtReader = new TxtReader(pathValidator);
        }

        [Test]
        public void ReadFile_Should_ReturnResultFail_When_FileNotExists()
        {
            var currentFilePath = Path.GetRandomFileName();
            var result = txtReader.ReadFile(currentFilePath);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ReadFile_Should_ReturnFileContent()
        {
            var currentFilePath = Path.GetRandomFileName() + ".txt";
            var words = new List<string>() { "съешь", "ещё", "этих", "мягких", "французских", "булок", "да", "выпей", "чаю" };
            var inputString = string.Join(Environment.NewLine, words);
            File.WriteAllText(currentFilePath, inputString);
            var result = txtReader.ReadFile(currentFilePath);
            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Should().BeEquivalentTo(inputString);
            File.Delete(currentFilePath);
        }

        [Test]
        public void ReadFile_Should_ReturnResultFail_When_ExtensionNotSupported()
        {
            var currentFilePath = Path.GetRandomFileName() + ".doc";
            File.Create(currentFilePath).Close();
            var result = txtReader.ReadFile(currentFilePath);
            result.IsSuccess.Should().BeFalse();
            File.Delete(currentFilePath);
        }
    }
}
