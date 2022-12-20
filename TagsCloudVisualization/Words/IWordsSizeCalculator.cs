namespace TagsCloudVisualization.Words;

public interface IWordsSizeCalculator
{
    Result<Dictionary<string, float>> CalcSizeForWords(Dictionary<string, int> wordsAndCount, float minFontSize, float maxFontSize);
}