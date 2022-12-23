using System.Drawing;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.CloudCreatorDriver.CloudDrawers.DrawingSettings;

public interface IDrawingSettings
{
    Color BgColor { get; set; }
    Size PictureSize { get; set; }

    bool HasWordVisualisationSelector();

    Result<IWordsVisualisationSelector> GetSelector();

    Result<IDrawingWord> GetDrawingWordFromSelector(IWord word, Rectangle rectangle);

    Result<IEnumerable<IWordVisualisation>> GetWordVisualisations();

    Result<IWordVisualisation> GetDefaultVisualisation();
}