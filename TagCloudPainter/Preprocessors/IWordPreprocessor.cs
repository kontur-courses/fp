using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Preprocessors;

public interface IWordPreprocessor
{
    Result<Dictionary<string, int>> GetWordsCountDictionary(IEnumerable<string> words);
}