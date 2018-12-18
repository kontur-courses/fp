using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Core.WordsParsing.WordsReading;

namespace TagCloud.Tests.TextReaders
{
    [TestFixture]
    public class TxtWordsReader_Should
    {
        private TxtWordsReader reader;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            reader = new TxtWordsReader();
        }

        [Test]
        public void DivideWords_ByAnyNonLetterOrNonDigitCharacter()
        {
            var expectedRes = "qwertyuiopasdfghjklzxcvbnm1234567890".Select(c => c.ToString()).ToList();
            var data = Encoding.UTF8.GetBytes(
                "q w!e@r#t$y\nu^i*o(p)a_s\nd-f=g+h\\j|k\nl/z.x,c<v>b`n~m,\n1-2+3+4-5@6#7$8%9^0\n");
            var stream = new MemoryStream(data);
            
            var res = reader.ReadFrom(stream).GetValueOrThrow();
            
            res.Should().BeEquivalentTo(expectedRes);
        }

        [Test]
        public void ReadFewWords_WhenFileHasFewWords()
        {
            var expectedRes = new List<string> {"Hello", "world", "Heh"};
            var data = Encoding.UTF8.GetBytes("Hello world\nHeh\n");
            var stream = new MemoryStream(data);
            
            var res = reader.ReadFrom(stream).GetValueOrThrow();
            
            res.Should().BeEquivalentTo(expectedRes);
        }

        [Test]
        public void ReadLastWord_WhenNoEscapeCharInTheEnd()
        {
            var expectedRes = new List<string> {"hello", "world"};
            var data = Encoding.UTF8.GetBytes("hello world");
            var stream = new MemoryStream(data);
            
            var res = reader.ReadFrom(stream).GetValueOrThrow();
            
            res.Should().BeEquivalentTo(expectedRes);
        }

        [Test]
        public void ReadOneWord_WhenFileHasOnlyOneWord()
        {
            var expectedRes = new List<string> {"word"};
            var data = Encoding.UTF8.GetBytes("word");
            var stream = new MemoryStream(data);
            
            var res = reader.ReadFrom(stream).GetValueOrThrow();
            
            res.Should().BeEquivalentTo(expectedRes);
        }
    }
}