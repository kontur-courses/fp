using System;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.TextProcessing;

namespace TagsCloud_Test
{
    public class TxtFileProvider_Test
    {
        private TxtFileProvider _fileProvider;

        [SetUp]
        public void SetUp()
        {
            _fileProvider = new TxtFileProvider();
        }

        [Test]
        public void GetTxtFilePath_Failure_WhenFileDoesNotExist()
        {
            var filePathResult = _fileProvider.GetTxtFilePath("BadFileName");
            filePathResult.IsSuccess.Should().BeFalse();
            Console.WriteLine(filePathResult.Error);
        }

        [Test]
        public void GetTxtFilePath_IsSuccess_WhenFileExist()
        {
            var path = Path.Combine(Path.GetTempPath(), "test.txt");
            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            writer.WriteLine("Новый тестовый файл txt");
            _fileProvider.GetTxtFilePath(path).IsSuccess.Should().BeTrue();
            File.Delete(path);
        }
    }
}