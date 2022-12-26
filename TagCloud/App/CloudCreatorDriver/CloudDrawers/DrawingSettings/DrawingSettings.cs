using System.Drawing;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.CloudCreatorDriver.CloudDrawers.DrawingSettings;

public class DrawingSettings : IDrawingSettings
{
    private readonly IWordVisualisation defaultVisualisation;
    private readonly IWordsVisualisationSelector visualisationSelector;
    private readonly List<IWordVisualisation> wordVisualisations;

    public DrawingSettings(IWordVisualisation defaultVisualisation, IWordsVisualisationSelector visualisationSelector)
    {
        this.defaultVisualisation = defaultVisualisation;
        this.visualisationSelector = visualisationSelector;
        wordVisualisations = new List<IWordVisualisation> { defaultVisualisation };
    }

    public Color BgColor { get; set; }
    public Size PictureSize { get; set; }

    public bool HasWordVisualisationSelector()
    {
        return !visualisationSelector.Empty();
    }

    public Result<IWordsVisualisationSelector> GetSelector()
    {
        return HasWordVisualisationSelector()
            ? Result.Ok(visualisationSelector)
            : Result.Fail<IWordsVisualisationSelector>("Selector hasn't visualisation rules");
    }

    public Result<IDrawingWord> GetDrawingWordFromSelector(IWord word, Rectangle rectangle)
    {
        return HasWordVisualisationSelector()
            ? visualisationSelector.GetWordVisualisation(word, rectangle)
            : Result.Fail<IDrawingWord>("Selector hasn't visualisation rules");
    }

    public IEnumerable<IWordVisualisation> GetWordVisualisations()
    {
        return wordVisualisations;
    }

    public IWordVisualisation GetDefaultVisualisation()
    {
        return defaultVisualisation;
    }
}