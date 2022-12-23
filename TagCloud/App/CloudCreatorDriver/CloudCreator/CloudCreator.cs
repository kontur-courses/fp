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
    private readonly FromFileInputWordsStream inputWordsStream;
    private readonly IWordsPreprocessor wordsPreprocessor;
    private readonly List<IBoringWords> boringWords;
    private readonly ICloudLayouter cloudLayouter;
    private readonly ICloudLayouterSettings cloudLayouterSettings;
    private readonly ICloudDrawer cloudDrawer;
    private readonly IDrawingSettings drawingSettings;

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
        List<IWord>? uniqueWords = null;
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
        
        return settings.GetSelector()
            .Then(selector =>
            {
                var minTf = words.Min(word => word.Tf);
                var maxTf = words.Max(word => word.Tf);
                selector.SetMinAndMaxRealWordTfIndex(minTf, maxTf);
            })
            .Then(_ => words);
    }

    private static Result<IEnumerable<Size>> GetWordsSizes(
        IEnumerable<IWord> words,
        IDrawingSettings drawingSettings)
    {
        return Result.Of(() =>  
            drawingSettings.HasWordVisualisationSelector()
            ? words.Select(word => drawingSettings.GetDrawingWordFromSelector(word, Rectangle.Empty)
                    .Then(w => w.Font)
                    .Then(word.MeasureWord)
                    .GetValueOrThrow())
            : words.Select(word => GetVisualisation(word, drawingSettings)
                .Then(w => w.Font)
                .Then(word.MeasureWord)
                .GetValueOrThrow()))
            .RefineError("Can not get word size");
    }

    private static Result<List<IWord>> GetProcessedWordsOrderedByTf(
        List<string> allWordsFromStream,
        IWordsPreprocessor wordsPreprocessor,
        IReadOnlyCollection<IBoringWords> boringWords)
    {
        return wordsPreprocessor.GetProcessedWords(allWordsFromStream, boringWords)
            .Then(words => Result.Of(() =>
                words.OrderByDescending(word => word.Tf).ToList()));
    }

    private static Result<List<IDrawingWord>> CreateDrawingWords(
        IEnumerable<IWord> words,
        IEnumerable<Rectangle> rectangles,
        IDrawingSettings drawingSettings)
    {
        var result = new List<IDrawingWord>();
        var enumerator = rectangles.GetEnumerator();
        foreach (var word in words)
        {
            if (!enumerator.MoveNext()) return Result.Fail<List<IDrawingWord>>("No rectangle for word");
            var drawingWord = drawingSettings.HasWordVisualisationSelector()
                ? drawingSettings.GetDrawingWordFromSelector(word, enumerator.Current)
                : GetVisualisation(word, drawingSettings)
                    .Then(stile => CreateDrawingWord(word, stile, enumerator.Current));

            if (!drawingWord.Then(dw => result.Add(dw)).IsSuccess)
                return Result.Fail<List<IDrawingWord>>("");
        }
        enumerator.Dispose();

        return Result.Ok(result);
    }

    private static IDrawingWord CreateDrawingWord(IWord word, IWordVisualisation stile, Rectangle rectangle)
    {
        return new DrawingWord(word, stile.Font, stile.Color, rectangle);
    }

    private static Result<IWordVisualisation> GetVisualisation(IWord word, IDrawingSettings drawingSettings)
    {
        return drawingSettings.GetWordVisualisations()
            .Then(v => Result.Of(() =>
                v.OrderByDescending(visualisation => visualisation.StartingValue)
                    .FirstOrDefault(visualisation => visualisation.StartingValue <= word.Tf)))
            .Then(v => v == null
                ? drawingSettings.GetDefaultVisualisation()
                : Result.Ok(v))
            .RefineError("Can not get visualisation");
    }
}