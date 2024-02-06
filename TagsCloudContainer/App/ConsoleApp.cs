using System.Drawing.Imaging;
using TagsCloudContainer.App.Extensions;
using TagsCloudContainer.App.Interfaces;
using TagsCloudContainer.DrawRectangle.Interfaces;
using TagsCloudContainer.FileReader;
using TagsCloudContainer.WordProcessing;
namespace TagsCloudContainer.App;

public class ConsoleApp : IApp
{
    private readonly FileReaderFactory _readerFactory;
    private readonly Settings _settings;
    private readonly WordProcessor _processor;
    private readonly IDraw _draw;

    public ConsoleApp(FileReaderFactory readerFactory, Settings settings, WordProcessor processor,
        IDraw draw)
    {
        _readerFactory = readerFactory;
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _processor = processor;
        _draw = draw;
    }
    
    public Result<None> Run()
    {
        var text = GetText(_settings.File).OnFail(Exit);
        var boringText = GetText(_settings.BoringWordsFileName).OnFail(Exit);
        var words = _processor.ProcessWords(text.Value, boringText.Value);
        var bitmap = _draw.CreateImage(words).OnFail(Exit);
        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        var rnd = new Random();
        bitmap.Value.Save(projectDirectory + "\\Images", $"Rectangles{rnd.Next(1, 1000)}", GetImageFormat(_settings.ImageFormat).GetValueOrThrow());
        return new Result<None>(null);
    }

    private Result<ImageFormat> GetImageFormat(string imageFormat)
    {
        return imageFormat.ToLower() switch
        {
            "png" =>  ImageFormat.Png.Ok(),
            "jpeg" => ImageFormat.Jpeg.Ok(),
            _ => Result.Fail<ImageFormat>($"Неверный формат изображения: {imageFormat}")
        };
    }
    
    private Result<string> GetText(string filename)
    {
        if (!File.Exists(filename))
            return Result.Fail<string>($"Файл не найден {filename}");
        var result = _readerFactory
            .GetReader(filename)
            .Then(x => x.GetTextFromFile(filename))
            .RefineError("Не удается получить текст из файла");
        return result;
    }

    private void Exit(string text)
    {
        Console.WriteLine(text);
        Environment.Exit(1);
    }
}