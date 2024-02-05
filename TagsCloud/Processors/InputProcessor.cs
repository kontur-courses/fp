using Microsoft.Extensions.DependencyInjection;
using TagsCloud.Contracts;
using TagsCloud.Conveyors;
using TagsCloud.CustomAttributes;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Results;
using TagsCloud.TextAnalysisTools;
using TagsCloudVisualization;

namespace TagsCloud.Processors;

[Injection(ServiceLifetime.Singleton)]
public class InputProcessor : IInputProcessor
{
    private readonly IEnumerable<IFileReader> fileReaders;
    private readonly FilterConveyor filterConveyor;
    private readonly IInputProcessorOptions inputOptions;
    private readonly IPostFormatter postFormatter;

    public InputProcessor(
        IInputProcessorOptions inputOptions,
        IEnumerable<IFileReader> fileReaders,
        IEnumerable<IFilter> filters,
        IPostFormatter postFormatter)
    {
        this.inputOptions = inputOptions;
        this.fileReaders = fileReaders;
        this.postFormatter = postFormatter;
        filterConveyor = new FilterConveyor(filters, inputOptions);
    }

    public Result<HashSet<WordTagGroup>> CollectWordGroupsFromFile(string filename)
    {
        if (!File.Exists(filename))
            return ResultExtensions.Fail<HashSet<WordTagGroup>>($"File {filename} not found!");

        var groupsResult = FindFileReader(filename)
                           .Then(reader => ExtractGroupsFromFile(filename, reader))
                           .Then(CheckForEmptyGroups)
                           .Then(TextAnalyzer.FillWithAnalysis)
                           .Then(filterConveyor.ApplyFilters)
                           .Then(CheckForEmptyGroups)
                           .Then(CastWordsToAppropriateForm)
                           .Then(CastWordsToAppropriateCase)
                           .Then(RegroupWords);

        return groupsResult;
    }

    private HashSet<WordTagGroup> CastWordsToAppropriateForm(HashSet<WordTagGroup> wordGroups)
    {
        foreach (var group in wordGroups.Where(group => group.WordInfo.IsRussian))
            group.WordInfo.Text = inputOptions.ToInfinitive ? group.WordInfo.Infinitive : group.WordInfo.Text;

        return wordGroups;
    }

    private HashSet<WordTagGroup> CastWordsToAppropriateCase(HashSet<WordTagGroup> wordGroups)
    {
        foreach (var group in wordGroups)
            group.WordInfo.Text = inputOptions.WordsCase == CaseType.Upper
                ? group.WordInfo.Text.ToUpper()
                : group.WordInfo.Text.ToLower();

        return wordGroups;
    }

    private Result<HashSet<WordTagGroup>> ExtractGroupsFromFile(string filename, IFileReader reader)
    {
        try
        {
            return reader
                   .ReadContent(filename, postFormatter)
                   .Where(line => !string.IsNullOrEmpty(line))
                   .GroupBy(line => line)
                   .Select(group => new WordTagGroup(group.Key, group.Count()))
                   .ToHashSet();
        }
        catch
        {
            return ResultExtensions
                .Fail<HashSet<WordTagGroup>>("Readonly file or incorrect permissions!");
        }
    }

    private string GetSupportedExtensions()
    {
        return string.Join(", ", fileReaders.Select(reader => reader.SupportedExtension));
    }

    private Result<IFileReader> FindFileReader(string filename)
    {
        var extension = GetFileExtension(filename);
        var reader = fileReaders.SingleOrDefault(reader => reader.SupportedExtension.Equals(extension));

        if (reader != null)
            return reader.AsResult();

        var extensions = GetSupportedExtensions();

        return ResultExtensions
            .Fail<IFileReader>($"Unknown file extension: {extension}! Candidates are: {extensions}");
    }

    private static string GetFileExtension(string filename)
    {
        return filename.Split('.', StringSplitOptions.RemoveEmptyEntries)[^1];
    }

    private static Result<HashSet<WordTagGroup>> RegroupWords(HashSet<WordTagGroup> wordGroups)
    {
        return wordGroups
               .GroupBy(group => group.WordInfo.Text)
               .Select(group =>
                   new WordTagGroup(group.Key, group.Sum(tag => tag.Count)))
               .ToHashSet();
    }

    private static Result<HashSet<WordTagGroup>> CheckForEmptyGroups(HashSet<WordTagGroup> wordGroups)
    {
        return wordGroups.Count == 0
            ? ResultExtensions.Fail<HashSet<WordTagGroup>>("Can't generate TagCloud from void!")
            : wordGroups;
    }
}