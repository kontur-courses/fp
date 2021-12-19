using NUnit.Framework;
using TagsCloudContainer.Parsers;

namespace TagsCloudContainerTests.ParserTests
{
    internal class DocParserTests : ParserTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            parser = new DocParser();
            format = "docx";
        }
    }
}