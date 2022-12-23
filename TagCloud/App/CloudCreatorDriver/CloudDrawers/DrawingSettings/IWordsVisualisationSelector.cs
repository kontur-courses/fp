using System.Drawing;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.CloudCreatorDriver.CloudDrawers.DrawingSettings;

public interface IWordsVisualisationSelector
{
    Result<IDrawingWord> GetWordVisualisation(IWord word, Rectangle rectangle);

    Result<None> AddWordPossibleColors(IEnumerable<Color> colors);

    Result<None> SetWordsSizes(int min, int max);

    Result<None> SetMinAndMaxRealWordTfIndex(double min, double max);

    bool Empty();
}