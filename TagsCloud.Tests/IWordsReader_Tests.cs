using System;
using NUnit.Framework;
using TagsCloud.FileReader;

namespace TagsCloud.Tests
{
    internal class IWordsReader_Tests
    {
        private IWordsReader txtReader = new TxtReader();
        private IWordsReader rtfReader = new RtfReader();
        private IWordsReader docxReader = new DocxReader();

        [Test]
        public void ReadWords_TxtReaderThrowException_WhenInvalidPathToFile()
        {
            Assert.Throws<InvalidOperationException>(() => txtReader.ReadWords("wafwafa").GetValueOrThrow());
        }

        [Test]
        public void ReadWords_DocxReaderThrowException_WhenInvalidPathToFile()
        {
            Assert.Throws<InvalidOperationException>(() => docxReader.ReadWords("wafwafa").GetValueOrThrow());
        }

        [Test]
        public void ReadWords_RtfReaderThrowException_WhenInvalidPathToFile()
        {
            Assert.Throws<InvalidOperationException>(() => rtfReader.ReadWords("wafwafa").GetValueOrThrow());
        }
    }
}
