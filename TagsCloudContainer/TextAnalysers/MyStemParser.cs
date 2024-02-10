namespace TagsCloudContainer.TextAnalysers;

public class MyStemParser : IMyStemParser
{
    public Result<WordDetails> Parse(string wordInfo)
    {
        var wordInfoSegments = ParseWordInfo(wordInfo);
        if (wordInfoSegments is null)
            return Result.Fail<WordDetails>("Ошибка при обработке слова."  + wordInfo);

        var word = wordInfoSegments[0];
        if (word.Contains("??"))
            return Result.Fail<WordDetails>("Ошибка при обработке слова." + wordInfo);

        var speechPart = wordInfoSegments[1]
            .Split(',')
            .First();

        return new WordDetails(word, speechPart: speechPart);
    }

    private string[]? ParseWordInfo(string wordInfo)
    {
        var wordInfoSegments = wordInfo.Split('=');
        return wordInfoSegments.Length >= 2 ? wordInfoSegments : null;
    }
}