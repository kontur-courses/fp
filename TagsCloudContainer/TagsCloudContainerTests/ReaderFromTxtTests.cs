using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Reader;

namespace TagsCloudContainerTests
{
    public class ReaderFromTxtTests
    {
        [Test]
        public void GetWordsSetReturnLineFile()
        {
            var pathTempFile = Path.GetTempFileName();
            try
            {
                using (var sw = File.CreateText(pathTempFile))
                {
                    sw.WriteLine("Это пример текстового файла.\nЭто вторая строка текстового файла?");
                }

                var reader = new ReaderFromTxt();
                reader.GetWordsSet(pathTempFile).GetValueOrThrow().ToArray().Should()
                    .BeEquivalentTo("Это пример текстового файла.", "Это вторая строка текстового файла?");
            }
            finally
            {
                File.Delete(pathTempFile);
            }
        }
        
        [TestCase("", TestName = "Empty string")]
        [TestCase(null, TestName = "Null")]
        [TestCase("*", TestName = "Path is \"*\"")]
        public void GetWordsSetReturnResultError_WhenThereIsNoFile(string path)
        {
            var reader = new ReaderFromTxt();
            reader.GetWordsSet(path).IsSuccess.Should().BeFalse();
        }
    }
}