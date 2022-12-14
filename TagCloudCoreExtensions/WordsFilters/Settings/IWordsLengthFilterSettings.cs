namespace TagCloudCoreExtensions.WordsFilters.Settings;

public interface IWordsLengthFilterSettings
{
    int MinWordLength { get; set; }
    int MaxWordLength { get; set; }
}