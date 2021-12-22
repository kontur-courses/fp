using System.IO;
using System.Text;
using CTV.Common.Readers;
using FluentAssertions;
using FunctionalProgrammingInfrastructure;
using NUnit.Framework;

namespace CTV.Tests
{
    [TestFixture]
    public class TxtFileReader_Should
    {
        [TestCase("", TestName ="When input is empty")]
        [TestCase("hello world", TestName = "When input is not empty")]
        public void ReturnWholeString_When(string wholeString)
        {
            var expected = wholeString.AsResult();
            var stringAsBytes = Encoding.Default.GetBytes(wholeString);
            var memoryStream = new MemoryStream(stringAsBytes);
            var fileReader = new TxtFileReader();

            var result = fileReader.ReadToEnd(memoryStream);
            result.Should().BeEquivalentTo(expected,
                config => config.ComparingByMembers<Result<string>>());
        }
        
        [Test]
        public void Fail_WhenInputStreamAlreadyWasClosed()
        {
            
            var memoryStream = new MemoryStream(Encoding.Default.GetBytes("abc"));
            memoryStream.Close();
            var fileReader = new TxtFileReader();

            var result = fileReader.ReadToEnd(memoryStream);
            result.IsSuccess.Should().BeFalse();
        }
    }
}