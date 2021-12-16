using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Infrastructure.FileReader;
using TagCloud.Infrastructure.Monad;

namespace TagCloudTests.Infrastructure.FileReader;

internal class FileReaderFactoryTests
{
    public static IFileReader DocReader = new DocFileReader();
    public static IFileReader PlainReader = new PlainTextFileReader();

    public static IEnumerable<TestCaseData> ReaderCaseDatas
    {
        get
        {
            yield return new TestCaseData("a.txt", Result.Ok(PlainReader))
                .SetName("ShouldReturnReader_WhenInputIsTxt");
            yield return new TestCaseData("a.unknownformat", Result.Fail<IFileReader>("Unsupported input file extension"))
                .SetName("ShouldReturnFail_WhenInputIsUnknown");
            yield return new TestCaseData("a.doc", Result.Ok(DocReader))
                .SetName("ShouldReturnReader_WhenInputIsDoc");
            yield return new TestCaseData("a.docx", Result.Ok(DocReader))
                .SetName("ShouldReturnReader_WhenInputIsDocx");
        }
    }

    private readonly FileReaderFactory sut = new(new[] { DocReader, PlainReader });

    [TestCaseSource(typeof(FileReaderFactoryTests), nameof(ReaderCaseDatas))]
    public void Create(string inputPath, Result<IFileReader> expected)
    {
        var actual = sut.Create(inputPath);

        actual.Should().BeEquivalentTo(expected);
    }
}