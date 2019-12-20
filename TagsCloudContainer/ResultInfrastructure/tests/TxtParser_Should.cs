using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Parsing;

namespace TagsCloudContainer.ResultInfrastructure.tests
{
    public class TxtParser_Should
    {
        [Test]
        public void Fail_WhenFileNotExist()
        {
            var parser = new TxtParser();
            var fileName = "thisfiledoesnotexist.txt";
            parser.ParseFile(fileName).IsSuccess.Should().BeFalse();
        }
    }
}