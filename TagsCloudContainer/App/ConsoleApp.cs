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
        DrawImage.CreateImage(Settings, Draw).Value.Save(path);
        
        return new Result<None>(null);
    }
}
