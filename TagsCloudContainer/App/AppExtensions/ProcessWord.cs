using TagsCloudContainer.FileReader;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.App.AppExtensions;

public class ProcessWord
{
    private Result<string> GetText(string filename)
    {
        if (!File.Exists(filename))
        {
            return Result.Fail<string>($"Файл не найден {filename}");
        }
        
        var result = new FileReaderFactory()
            .GetReader(filename)
            .Then(x => x.GetTextFromFile(filename))
            .RefineError("Не удается получить текст из файла");
        return result;
    }
    
    public List<Word> GetProcessWords(string text, string boringText, Settings settings)
    {
        var resultText = GetText(text).OnFail(Exit);
        var resultBoringText = GetText(boringText).OnFail(Exit);
        return new WordProcessor(settings).ProcessWords(resultText.Value, resultBoringText.Value);
    }
    
    public static void Exit(string text)
    {
        Console.WriteLine(text);
        Environment.Exit(1);
    }
}