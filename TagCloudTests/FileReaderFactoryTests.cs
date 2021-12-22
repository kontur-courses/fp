using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Readers;
using TextReader = TagCloud.Readers.TextReader;

namespace TagCloudTests
{
    public class FileReaderFactoryTests
    {
        private IFileReaderFactory readerFactory;

        [SetUp]
        public void SetUp()
        {
            readerFactory = new FileReaderFactory();
        }

        [Test]
        public void Create_ShouldReturnsFailResult_WhenExtensionNotSupported()
        {
            readerFactory.Create("abc").Error.Should().NotBeNullOrEmpty();
        }

        [TestCase("txt", typeof(TextReader))]
        [TestCase("xml", typeof(XmlFileReader))]
        [TestCase("docx", typeof(DocFileReader))]
        public void Create_ShouldReturnReader(string extension, Type expectedType)
        {
            var lines = readerFactory.Create(extension);

            lines.GetValueOrThrow().Should().BeOfType(expectedType);
        }
    }
}
