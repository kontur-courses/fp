using System.Drawing.Imaging;
using TagsCloudContainer.DrawRectangle;
using TagsCloudContainer.FileReader;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.App;

public class ConsoleApp : IApp
{
    private  FileReaderFactory ReaderFactory { get; }
    private Settings Settings { get; }
    private  WordProcessor Processor { get; }
    private  IDraw Draw { get; }

    public ConsoleApp(FileReaderFactory readerFactory, Settings settings, WordProcessor processor,
        IDraw draw)
    {
        ReaderFactory = readerFactory;
        Settings = settings;
        Processor = processor;
        Draw = draw;
    }
    
    public Result<None> Run()
    {
        var path = GetPathForSaveImage();
        var words = GetProcessWords(Settings.File, Settings.BoringWordsFileName);
        var bitmap = Draw.CreateImage(words).OnFail(Exit);
        bitmap.Value.Save(path);
        
        return new Result<None>(null);
    }
    
    private string GetPathForSaveImage()
    {
        var imagesDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Images";
        var randomImagesSuffix = new Random().Next(1, 1000);
        var imageFormat = GetImageFormat(Settings.ImageFormat).GetValueOrThrow();
        var path = Path.Combine(imagesDirectory, $"Rectangle{randomImagesSuffix}.{imageFormat}");
        return path;
    }
    private List<Word> GetProcessWords(string text, string boringText)
    {
        var resultText = GetText(text).OnFail(Exit);
        var resultBoringText = GetText(boringText).OnFail(Exit);
        return Processor.ProcessWords(resultText.Value, resultBoringText.Value);
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
        {
            return Result.Fail<string>($"Файл не найден {filename}");
        }
        
        var result = ReaderFactory
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
