using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Core.Util;
using TagCloud.Core.WordsParsing.WordsReading;

namespace TagCloud.Tests.TextReaders
{
    [TestFixture]
    public class XmlWordsReader_Should
    {
        private XmlWordsReader reader;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            reader = new XmlWordsReader();
        }

        [Test]
        public void ReadFewWords_WhenFileHasFewWords()
        {
            var expectedRes = new List<string> {"word1", "word2", "word3"};
            var data = Encoding.UTF8.GetBytes(WrapWordsWithXml(expectedRes));
            var stream = new MemoryStream(data);
            
            var res = reader.ReadFrom(stream).GetValueOrThrow();
            
            res.Should().BeEquivalentTo(expectedRes);
        }

        [Test]
        public void ReadOneWord_WhenFileHasOnlyOneWord()
        {
            var expectedRes = new List<string> {"word"};
            var data = Encoding.UTF8.GetBytes(WrapWordsWithXml(expectedRes));
            var stream = new MemoryStream(data);
            
            var res = reader.ReadFrom(stream).GetValueOrThrow();
            
            res.Should().BeEquivalentTo(expectedRes);
        }

        [Test]
        public void ReadZeroWordsWithoutExceptions_WhenFileHasZeroWords()
        {
            var data = Encoding.UTF8.GetBytes(WrapWordsWithXml(new string[0]));
            var stream = new MemoryStream(data);
            
            var res = reader.ReadFrom(stream).GetValueOrThrow();
            
            res.Should().NotBeNull();
            res.Count().Should().Be(0);
        }

        private string WrapWordsWithXml(IEnumerable<string> words)
        {
            var res = new StringBuilder();
            res.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n\n<ArrayOfString>\n");
            words.ApplyForeach(word => res.Append($"<string>{word}</string>\n"));
            res.Append("</ArrayOfString>");
            return res.ToString();
        }
    }
}