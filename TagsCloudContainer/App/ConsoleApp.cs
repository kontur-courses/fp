using TagsCloudContainer.App.AppExtensions;
using TagsCloudContainer.DrawRectangle;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.App;

public class ConsoleApp : IApp
{
    private Settings Settings { get; }
    private  IDraw Draw { get; }

    public ConsoleApp(Settings settings, IDraw draw)
    {
        Settings = settings;
        Draw = draw;
    }
    
    public Result<None> Run()
    {
        var path = new PathImage().GetPathForSaveImage(Settings);
        var words = new ProcessWord().GetProcessWords(Settings.File, Settings.BoringWordsFileName, Settings);
        var bitmap = Draw.CreateImage(words).OnFail(Exit);
        bitmap.Value.Save(path);
        
        return new Result<None>(null);
    }
    
    private void Exit(string text)
    {
        Console.WriteLine(text);
        Environment.Exit(1);
    }
}
