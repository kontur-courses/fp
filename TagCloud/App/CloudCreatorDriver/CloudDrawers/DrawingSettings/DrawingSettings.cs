﻿using System.Drawing;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.CloudCreatorDriver.CloudDrawers.DrawingSettings;

public class DrawingSettings : IDrawingSettings
{
    public Color BgColor { get; set; }
    public Size PictureSize { get; set; }
    
    private readonly IWordVisualisation defaultVisualisation;
    private readonly IWordsVisualisationSelector visualisationSelector;
    private readonly List<IWordVisualisation> wordVisualisations;

    public DrawingSettings(IWordVisualisation defaultVisualisation, IWordsVisualisationSelector visualisationSelector)
    {
        this.defaultVisualisation = defaultVisualisation;
        this.visualisationSelector = visualisationSelector;
        wordVisualisations = new List<IWordVisualisation>(){defaultVisualisation};
    }

    public bool HasWordVisualisationSelector() => !visualisationSelector.Empty();
    public Result<IWordsVisualisationSelector> GetSelector()
    {
        return HasWordVisualisationSelector()
            ? Result.Ok(visualisationSelector)
            : Result.Fail<IWordsVisualisationSelector>("Selector was not initialised");
    }

    public Result<IDrawingWord> GetDrawingWordFromSelector(IWord word, Rectangle rectangle)
    {
        return HasWordVisualisationSelector()
            ? visualisationSelector.GetWordVisualisation(word, rectangle)
            : Result.Fail<IDrawingWord>("Selector was not initialised");
    }

    public Result<IEnumerable<IWordVisualisation>> GetWordVisualisations()
    {
        return wordVisualisations;
    }

    public Result<IWordVisualisation> GetDefaultVisualisation() => Result.Ok(defaultVisualisation);
}