using System.Drawing;
using TagCloud.App.CloudCreatorDriver.CloudCreator;
using TagCloud.App.CloudCreatorDriver.CloudDrawers.DrawingSettings;
using TagCloud.App.CloudCreatorDriver.ImageSaver;
using TagCloud.App.CloudCreatorDriver.RectanglesLayouters;
using TagCloud.App.CloudCreatorDriver.RectanglesLayouters.SpiralCloudLayouters;
using TagCloud.App.WordPreprocessorDriver.InputStream;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.BoringWords;

namespace TagCloud.Clients.ConsoleClient;

public class ConsoleClient : IClient
{
    private readonly ICloudLayouterSettings cloudLayouterSettings;

    private readonly ICloudCreator creator;
    private readonly IDrawingSettings drawingSettings;
    private readonly IReadOnlyCollection<IFileEncoder> fileEncoders;
    private readonly IImageSaver imageSaver;
    private readonly FromFileInputWordsStream inputWordsStream;
    private readonly IWordsVisualisationSelector wordsVisualisationSelector;
    private Color bgColor;
    private Size imageSize;
    private string? pathToText;
    private string? savePath;

    public ConsoleClient(
        IReadOnlyCollection<IFileEncoder> fileEncoders,
        ICloudLayouterSettings cloudLayouterSettings,
        IDrawingSettings drawingSettings,
        ICloudCreator cloudCreator,
        IImageSaver imageSaver, FromFileInputWordsStream inputWordsStream,
        IWordsVisualisationSelector wordsVisualisationSelector)
    {
        this.fileEncoders = fileEncoders;
        this.cloudLayouterSettings = cloudLayouterSettings;
        this.drawingSettings = drawingSettings;
        creator = cloudCreator;
        this.imageSaver = imageSaver;
        this.inputWordsStream = inputWordsStream;
        this.wordsVisualisationSelector = wordsVisualisationSelector;
    }

    public Result<None> Run()
    {
        return Start()
            .Then(_ => GetFilePath().Then(p => pathToText = p))
            .Then(_ => GetImageSize().Then(s => imageSize = s))
            .Then(_ => GetBgColor().Then(c => bgColor = c))
            .Then(_ => GetOutImagePath().Then(p => savePath = p))
            .Then(_ => Result.OfAction(() =>
            {
                drawingSettings.BgColor = bgColor;
                drawingSettings.PictureSize = imageSize;
                ((SpiralCloudLayouterSettings)cloudLayouterSettings).Center =
                    new Point(imageSize.Width / 2, imageSize.Height / 2);
            }))
            .Then(_ => WriteToConsole(Phrases.AskingAddingUsersBoringWords))
            .Then(_ => ReadLineFromConsole())
            .Then(answer => answer == Phrases.Yes
                ? GetBoringWords().Then(bw => creator.AddBoringWordManager(bw))
                : Result.Ok())
            .Then(_ => CollectWordsColors(wordsVisualisationSelector))
            .Then(_ => CollectFontSizes(wordsVisualisationSelector))
            .Then(_ => GetFileEncoder(fileEncoders, pathToText!))
            .Then(encoder => Result.Of(() => new FromFileStreamContext(pathToText!, encoder)))
            .Then(context => creator.CreatePicture(context))
            .Then(image => imageSaver.SaveImage(image, savePath!))
            .Then(_ => WriteLineToConsole(Phrases.SuccessSaveImage + savePath))
            .RefineError("Program finished with error")
            .OnFail(e => WriteLineToConsole(e));
    }

    private static Result<None> CollectFontSizes(IWordsVisualisationSelector selector)
    {
        return WriteToConsole(Phrases.AskingFontSize)
            .Then(_ => ReadLineFromConsole())
            .Then(font => Result.Of(() =>
                    font.Split(' ')
                        .Select(int.Parse)
                        .Take(2).ToArray())
                .RefineError("Can not recognize font sizes"))
            .Then(sizes => selector.SetWordsSizes(sizes[0], sizes[1]))
            .RefineError("Can not get font sizes from console")
            .ReturnOnFail(e => TryAgain(e, () => CollectFontSizes(selector)));
    }

    private static Result<None> CollectWordsColors(IWordsVisualisationSelector visualisationSelector)
    {
        return WriteToConsole(Phrases.AskingWordsColors)
            .Then(_ => ReadLineFromConsole())
            .Then(colors => Result.Of(() =>
                    colors.Split('-')
                        .Select(Color.FromName)
                        .Where(cColor => cColor.IsKnownColor)
                        .ToArray())
                .RefineError("Can not recognize colors from input"))
            .Then(colors => FailIf(colors, colors.Length == 0, "Colors count can not be 0"))
            .Then(visualisationSelector.AddWordPossibleColors)
            .RefineError("Can not get colors from console")
            .ReturnOnFail(e => TryAgain(e, () => CollectWordsColors(visualisationSelector)));
    }

    private Result<BoringWordsFromUser> GetBoringWords()
    {
        return WriteLineToConsole(Phrases.AskingFullPathToBoringWords)
            .Then(_ => GetFilePathFromConsole())
            .Then(GetWordsFromFile)
            .Then(CreateBoringWords)
            .RefineError("Can not get users boring words")
            .ReturnOnFail(e => TryAgain(e, GetBoringWords));
    }

    private Result<BoringWordsFromUser> CreateBoringWords(IEnumerable<string> words)
    {
        var boringWord = new BoringWordsFromUser();
        return Result.OfAction(() =>
            {
                foreach (var word in words) boringWord.AddBoringWord(word);
            })
            .Then(_ => boringWord)
            .RefineError("Can not create boring words class from words");
    }

    private Result<List<string>> GetWordsFromFile(string fileName)
    {
        return GetFileEncoder(fileEncoders, fileName)
            .Then(fileEncoder => new FromFileStreamContext(fileName, fileEncoder))
            .Then(context => inputWordsStream.GetAllWordsFromStream(context))
            .RefineError("Can not read data from boring words file");
    }

    private static Result<IFileEncoder> GetFileEncoder(IEnumerable<IFileEncoder> fileEncoders, string fileName)
    {
        return Result.Of(() =>
                fileEncoders.First(encoder =>
                    fileName.EndsWith(encoder.GetExpectedFileType())))
            .RefineError("Can not find file encoder");
    }

    private static Result<None> Start()
    {
        return WriteLineToConsole(Phrases.Hello);
    }

    private static Result<Color> GetBgColor()
    {
        return WriteToConsole(Phrases.AskingBgColor)
            .Then(_ => GetColorFromConsole())
            .ReturnOnFail(e => TryAgain(e, GetBgColor));
    }

    private static Result<Color> GetColorFromConsole()
    {
        return ReadLineFromConsole()
            .Then(color => Result.Of(() =>
                Color.FromName(color), "Can not recognize color"));
    }

    private static Result<Size> GetImageSize()
    {
        return WriteToConsole(Phrases.AskingImageSize)
            .Then(_ => GetImageSizeFromConsole())
            .ReturnOnFail(e => TryAgain(e, GetImageSize));
    }

    private static Result<Size> GetImageSizeFromConsole()
    {
        return ReadLineFromConsole()
            .Then(size => Result.Of(() => size.Split('*').Select(int.Parse).ToArray(), "Parse error"))
            .Then(size => FailIf(size, size.Length != 2, "Incorrect Size input"))
            .Then(size => new Size(size[0], size[1]));
    }

    private static Result<string> GetOutImagePath()
    {
        return WriteLineToConsole(Phrases.AskingFullPathToOutImage)
            .Then(_ => WriteToConsole(Phrases.GetArrow(0)))
            .Then(_ => ReadLineFromConsole())
            .ReturnOnFail(e => TryAgain(e, GetOutImagePath));
    }

    private static Result<string> GetFilePath()
    {
        return
            WriteLineToConsole(Phrases.AskingFullPathToText)
                .Then(_ => GetFilePathFromConsole())
                .ReturnOnFail(e => TryAgain(e, GetFilePath));
    }

    private static Result<string> GetFilePathFromConsole()
    {
        return WriteLineToConsole(Phrases.GetArrow(0))
            .Then(_ => ReadLineFromConsole())
            .Then(path => FailIf(path, !File.Exists(path), "File not exists"));
    }

    private static Result<None> WriteToConsole(string text)
    {
        return Result.OfAction(() => Console.Write(text), "Can not write text to console");
    }

    private static Result<None> WriteLineToConsole(string text)
    {
        return WriteToConsole(text + '\n');
    }

    private static Result<string> ReadLineFromConsole()
    {
        return Result.Of(Console.ReadLine, "Can not read data from console")
            .Then(str => FailIf(str, str == null, "Console input can not be null"));
    }

    private static Result<T> TryAgain<T>(string error, Func<Result<T>> function)
    {
        return WriteToConsole(error + ". " + Phrases.TryAgain)
            .Then(_ => ReadLineFromConsole())
            .Then(i => i == Phrases.Yes
                ? function()
                : Result.Fail<T>(error));
    }

    private static Result<T> FailIf<T>(T? value, bool checker, string error)
    {
        return value == null || checker
            ? Result.Fail<T>(error)
            : value.AsResult();
    }
}