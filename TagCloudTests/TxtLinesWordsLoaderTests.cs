using System.Collections.Concurrent;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class TxtLinesWordsLoaderTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        if (Directory.Exists(Dir))
            Directory.Delete(Dir, true);
        Directory.CreateDirectory(Dir);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (Directory.Exists(Dir))
            Directory.Delete(Dir, true);
    }

    [SetUp]
    public void SetUp()
    {
        var filepath = Path.Combine(Dir, $"{TestContext.CurrentContext.Test.Name}.txt");
        using (var file = File.Open(filepath, FileMode.OpenOrCreate))
            file.Flush();

        filepathById[TestContext.CurrentContext.Test.ID] = filepath;
    }

    private readonly ConcurrentDictionary<string, string> filepathById = new();
    private const string Dir = "TestTxtFiles";

    [Test]
    public void Constructor_ThrowFileNotFoundException_OnNonExistentFile()
    {
        var filepath = filepathById[TestContext.CurrentContext.Test.ID];
        File.Delete(filepath);

        var act = () => new TxtLinesWordsLoader(filepath);

        act.Should().Throw<FileNotFoundException>()
            .WithMessage($"Could not find file '{Path.GetFullPath(filepath)}'.");
    }

    [Test]
    public void Load_ThrowFileNotFoundException_IfFileDeletedAfterLoaderInit()
    {
        var filepath = filepathById[TestContext.CurrentContext.Test.ID];
        var loader = new TxtLinesWordsLoader(filepath);
        File.Delete(filepath);

        var act = () => loader.Load();

        act.Should().Throw<FileNotFoundException>()
            .WithMessage($"Could not find file '{Path.GetFullPath(filepath)}'.");
    }

    [Test]
    public void Load_ReturnEmptyEnumerable_OnEmptyFile()
    {
        var filepath = filepathById[TestContext.CurrentContext.Test.ID];
        var loader = new TxtLinesWordsLoader(filepath);

        var result = loader.Load();

        result.Should().BeEmpty();
    }

    [Test]
    public void Load_ReturnCorrectFileLines()
    {
        var filepath = filepathById[TestContext.CurrentContext.Test.ID];
        var lines = new[] { "Карл", "у", "Клары", "украл", "кораллы", "а", "Клара", "у", "Карла", "украла", "кларнет" };
        File.WriteAllLines(filepath, lines);
        var loader = new TxtLinesWordsLoader(filepath);

        var result = loader.Load();

        result.Should().Equal(lines);
    }

    [Test]
    public void Load_ReturnCorrectFileLines_AfterFileChanged()
    {
        var filepath = filepathById[TestContext.CurrentContext.Test.ID];
        var lines = new[] { "Карл", "у", "Клары", "украл", "кораллы", "а", "Клара", "у", "Карла", "украла", "кларнет" };
        File.WriteAllLines(filepath, lines);
        var loader = new TxtLinesWordsLoader(filepath);
        var resultBefore = loader.Load();
        var newLines = new[] { "От", "топота", "копыт", "пыль", "по", "полю", "летит" };
        File.WriteAllLines(filepath, newLines);

        var resultAfter = loader.Load();

        resultBefore.Should().Equal(lines);
        resultAfter.Should().Equal(newLines);
    }
}