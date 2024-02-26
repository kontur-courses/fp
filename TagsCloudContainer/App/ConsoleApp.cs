using TagsCloudContainer.App.AppExtensions;
using TagsCloudContainer.DrawRectangle;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.App;

public class ConsoleApp : IApp
{
    private Settings Settings { get; }
    private IDraw Draw { get; }

    public ConsoleApp(Settings settings, IDraw draw)
    {
        Settings = settings;
        Draw = draw;
    }

    public Result<None> Run()
    {
        return DrawImage.CreateImage(Settings, Draw)
            .Then(image => ImageWorker.SaveImage(image, Settings));
    }
}