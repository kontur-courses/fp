using MyStemWrapper;
using TagsCloudContainer.TextAnalysers.WordsFilters;

namespace TagsCloudContainer.TextAnalysers;

public class TextPreprocessor : ITextPreprocessor
{
    private readonly MyStem myStem;
    private readonly IMyStemParser myStemParser;
    private readonly IWordsFilter wordsFilter;
    private readonly IFrequencyCalculator frequencyCalculator;

    public TextPreprocessor(MyStem myStem, IMyStemParser myStemParser, IWordsFilter wordsFilter,
        IFrequencyCalculator frequencyCalculator)
    {
        this.myStem = myStem;
        this.myStemParser = myStemParser;
        this.wordsFilter = wordsFilter;
        this.frequencyCalculator = frequencyCalculator;
    }

    public Result<WordDetails[]> Preprocess(string text)
    {
        string analyzed;
        try
        {
            analyzed = myStem.Analysis(text);
        }
        catch (Exception e)
        {
            return Result.Fail<WordDetails[]>($"Ошибка при предобработке текста. {e.Message}");
        }

        var wordInfos = analyzed.Split('\n');
        var wordsDetails = new List<WordDetails>(wordInfos.Length);
        foreach (var wordInfo in wordInfos)
        {
            myStemParser.Parse(wordInfo)
                .Then(details => wordsDetails.Add(details));
        }

        return frequencyCalculator.CalculateFrequency(wordsDetails)
            .Then(wordsFilter.Filter)
            .Then(details => details.ToArray());
    }
}