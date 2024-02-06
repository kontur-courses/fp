using System.Diagnostics;
using System.Text.Json;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.TextAnalysisTools;

public class TextAnalyzer
{
    public static Result<HashSet<WordTagGroup>> FillWithAnalysis(HashSet<WordTagGroup> wordGroups)
    {
        return ResultExtensions
               .Of(() => GetTextAnalysis(wordGroups))
               .Then(analyses => FillGroups(wordGroups, analyses))
               .ReplaceError(_ => "Something went wrong in words analyzing process!");
    }

    private static List<WordSummary> GetTextAnalysis(HashSet<WordTagGroup> wordGroups)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "mystem",
            Arguments = "-i --format=json",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        };

        process.Start();

        foreach (var group in wordGroups)
            process.StandardInput.Write(group.WordInfo.Text + ' ');

        process.StandardInput.Close();

        var analyses = JsonSerializer
            .Deserialize<List<WordSummary>>(process.StandardOutput.ReadToEnd());

        process.WaitForExit();

        return analyses;
    }

    private static HashSet<WordTagGroup> FillGroups(HashSet<WordTagGroup> wordGroups, IList<WordSummary> analyses)
    {
        var analysisIndex = 0;

        foreach (var group in wordGroups)
        {
            var analysis = analyses[analysisIndex].Analyses.FirstOrDefault();

            if (analysis == null)
            {
                group.WordInfo.IsRussian = false;
            }
            else
            {
                group.WordInfo.Infinitive = analysis.Infinitive;
                group.WordInfo.LanguagePart = analysis.LanguagePart;
            }

            analysisIndex++;
        }

        return wordGroups;
    }
}