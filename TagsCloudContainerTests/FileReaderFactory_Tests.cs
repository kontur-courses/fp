using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.FileReaders;

namespace TagsCloudContainerTests
{
    public class FileReaderFactory_Tests
    {
        private readonly FileReaderFactory factory = new FileReaderFactory(new List<IFileReader>()
            { new TxtFileReader(), new DocFileReader() });

        [Test]
        public void GetProperFileReader_ReturnsFailResult_WhenFormatIsNotSupported()
        {
            var filePath = "fakeFile.rtf";

            var result = factory.GetProperFileReader(filePath);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be($".rtf format is not supported");
        }

        [Test]
        public void GetProperFileReader_ReturnsCorrectReader_WhenFormatIsSupported()
        {
            var filePath = "realFile.txt";

            var result = factory.GetProperFileReader(filePath);

            result.GetValueOrThrow().Should().BeEquivalentTo(new TxtFileReader());
        }
    }
}