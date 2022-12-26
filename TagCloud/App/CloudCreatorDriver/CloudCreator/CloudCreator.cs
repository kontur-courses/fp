using System.Drawing;
using TagCloud.App.CloudCreatorDriver.CloudDrawers;
using TagCloud.App.CloudCreatorDriver.CloudDrawers.DrawingSettings;
using TagCloud.App.CloudCreatorDriver.RectanglesLayouters;
using TagCloud.App.WordPreprocessorDriver.InputStream;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.BoringWords;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.CloudCreatorDriver.CloudCreator;

public class CloudCreator : ICloudCreator
{
    private readonly List<IBoringWords> boringWords;
    private readonly ICloudDrawer cloudDrawer;
    private readonly ICloudLayouter cloudLayouter;
    private readonly ICloudLayouterSettings cloudLayouterSettings;
    private readonly IDrawingSettings drawingSettings;
    private readonly FromFileInputWordsStream inputWordsStream;
    private readonly IWordsPreprocessor wordsPreprocessor;

    public CloudCreator(
        FromFileInputWordsStream inputWordsStream,
        IWordsPreprocessor wordsPreprocessor, IEnumerable<IBoringWords> boringWords,
        ICloudLayouter cloudLayouter, ICloudLayouterSettings cloudLayouterSettings,
        ICloudDrawer cloudDrawer, IDrawingSettings drawingSettings)
    {
        this.inputWordsStream = inputWordsStream;
        this.wordsPreprocessor = wordsPreprocessor;
        this.boringWords = boringWords.ToList();
        this.cloudLayouter = cloudLayouter;
        this.cloudLayouterSettings = cloudLayouterSettings;
        this.cloudDrawer = cloudDrawer;
        this.drawingSettings = drawingSettings;
    }

    public Result<None> AddBoringWordManager(IBoringWords boringWordsManager)
    {
        return Result.OfAction(() => boringWords.Add(boringWordsManager));
    }

    public Result<Bitmap> CreatePicture(FromFileStreamContext streamContext)
    {
        List<IWord> uniqueWords = new();
        return inputWordsStream.GetAllWordsFromStream(streamContext)
            .Then(words => GetProcessedWordsOrderedByTf(words, wordsPreprocessor, boringWords))
            .Then(words =>
            {
                uniqueWords = words;
                return SetTfValuesToSelector(words, drawingSettings);
            })
            .Then(words => GetWordsSizes(words, drawingSettings))
            .Then(sizes => cloudLayouter.GetLaidRectangles(sizes, cloudLayouterSettings))
            .Then(rectangles => CreateDrawingWords(uniqueWords!, rectangles, drawingSettings))
            .Then(words => cloudDrawer.DrawWords(words, drawingSettings));
    }

    private static Result<List<IWord>> SetTfValuesToSelector(List<IWord> words, IDrawingSettings settings)
    {
        if (!settings.HasWordVisualisationSelector())
            return Result.Fail<List<IWord>>("Word visualisation selector not found");
    
        var minTf = words.Min(word => word.Tf);
        var maxTf = words.Max(word => word.Tf);
        return settings.GetSelector()
            .Then(selector => selector.SetMinAndMaxRealWordTfIndex(minTf, maxTf))
            .Then(_ => words);
    }

    private static IEnumerable<Size> GetWordsSizes(
        IEnumerable<IWord> words,
        IDrawingSettings drawingSettings)
    {
        return drawingSettings.HasWordVisualisationSelector()
            ? words.Select(word => drawingSettings.GetDrawingWordFromSelector(word, Rectangle.Empty)
                .Then(w => w.Font)
                .Then(word.MeasureWord)
                .GetValueOrThrow()) // never throw or null in ths context
            : words.Select(word =>
            {
                var font = GetVisualisation(word, drawingSettings).Font;
                return word.MeasureWord(font);
            });
    }

    private static List<IWord> GetProcessedWordsOrderedByTf(
        List<string> allWordsFromStream,
        IWordsPreprocessor wordsPreprocessor,
        IReadOnlyCollection<IBoringWords> boringWords)
    {
        return wordsPreprocessor.GetProcessedWords(allWordsFromStream, boringWords)
            .OrderByDescending(word => word.Tf)
            .ToList();
    }

    private static List<IDrawingWord> CreateDrawingWords(
        IEnumerable<IWord> words,
        IEnumerable<Rectangle> rectangles,
        IDrawingSettings drawingSettings)
    {
        using var enumerator = rectangles.GetEnumerator();

        return words.TakeWhile(_ => enumerator.MoveNext())
            .Select(word => drawingSettings.GetDrawingWordFromSelector(word, enumerator.Current)
                .OnFailChangeTo(_ =>
                {
                    var stile = GetVisualisation(word, drawingSettings);
                    return CreateDrawingWord(word, stile, enumerator.Current).AsResult();
                }).GetValueOrThrow())
            .Select(drawingWord => drawingWord!)
            .ToList();
    }

    private static IDrawingWord CreateDrawingWord(IWord word, IWordVisualisation stile, Rectangle rectangle)
    {
        return new DrawingWord(word, stile.Font, stile.Color, rectangle);
    }

    private static IWordVisualisation GetVisualisation(IWord word, IDrawingSettings drawingSettings)
    {
        var resVisualisation = drawingSettings.GetWordVisualisations()
                .OrderByDescending(visualisation => visualisation.StartingValue)
                .FirstOrDefault(visualisation => visualisation.StartingValue <= word.Tf);
        return resVisualisation ?? drawingSettings.GetDefaultVisualisation();
    }
}