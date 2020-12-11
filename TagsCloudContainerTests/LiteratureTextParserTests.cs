using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.TextAnalyzer;

namespace TagsCloudContainerTests
{
    internal class LiteratureTextParserTests
    {
        [Test]
        public void LiteratureTextParser_ShouldParseTextToWords()
        {
            new LiteratureTextParser()
                .GetWords(new[] {"строка с символами, словами и не 6675 словами"})
                .ToArray()
                .Should()
                .BeEquivalentTo("строка", "с", "символами", "словами", "и", "не", "словами");
        }
    }
}