using NUnit.Framework;
using TagsCloudContainer.Parsers;

namespace TagsCloudContainerTests.ParserTests;

internal class TxtParserTests : ParserTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        parser = new TxtParser();
        format = "txt";
    }
}