using TagsCloud.Layouters;
using TagsCloud.TagsCloudPainters;
using TagsCloud.WordFontCalculators;
using TagsCloud.WordsProviders;

namespace TagsCloud.App;

public class App : IApp
{
    private readonly IWordsProvider _wordsProvider;
    private readonly IWordFontCalculator _fontCalculator;
    private readonly ILayouter _layouter;
    private readonly IPainter _painter;

    public App(IWordsProvider wordsProvider, IWordFontCalculator fontCalculator,ILayouter layouter, IPainter painter)
    {
        this._wordsProvider = wordsProvider;
        this._fontCalculator = fontCalculator;
        this._layouter = layouter;
        this._painter = painter;
    }

    public void Run()
    {
        var result = _wordsProvider.GetWords()
            .Then(_fontCalculator.GetWordsFont)
            .Then(_layouter.CreateTagsCloud).Then(_painter.DrawCloud);
        if (!result.IsSuccess)
        {
            Console.WriteLine(result.Error);
        }
    }
}