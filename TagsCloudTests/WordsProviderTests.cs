using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using ResultMonad;
using TagsCloudVisualization.WordsProvider;
using TagsCloudVisualization.WordsProvider.FileReader;

namespace TagsCloudTests
{
    public class WordsProviderTests
    {
        [Test]
        public void GetFileContent_ShouldBeResultFail_WhenFileDoesNotExists()
        {
            const string path = "someNotExistsFile.txt";
            var provider = new FileReadService(path, new []{ new TxtFileReader() });
            var actual = provider.GetFileContent();

            var expected = Result.Fail<IEnumerable<string>>($"File {path} not found");
            actual.Should().BeEquivalentTo(expected);
        }
    }
}