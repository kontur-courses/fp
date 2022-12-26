using System.Drawing;
using System.Drawing.Imaging;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Cli;

public class CliGraphicsProvider : IGraphicsProvider
{
    private readonly CliGraphicsProviderSettings settings;
    private Bitmap? bitmap;
    private Graphics? graphics;

    public CliGraphicsProvider(CliGraphicsProviderSettings settings)
    {
        this.settings = settings;
    }

    public Result<Graphics> Create()
    {
        return Commit()
            .BindTry(() =>
                Result.Success(new Bitmap(settings.Width, settings.Height)))
            .Tap(localBitmap => bitmap = localBitmap)
            .BindTry(image => Result.Success(Graphics.FromImage(image)))
            .Tap(localGraphics => graphics = localGraphics);
    }

    public Result Commit()
    {
        return Result.Success()
            .BindIf(graphics is not null, () => Result.Try(graphics!.Dispose))
            .BindIf(bitmap is not null, SaveBitmap)
            .Anyway(_ => bitmap = null)
            .Anyway(_ => graphics = null);
    }

    private Result SaveBitmap()
    {
        var cache = new MemoryStream();
        return Result.Try(() => bitmap!.Save(cache, ImageFormat.Png))
            .OnSuccessTry(() => bitmap!.Dispose())
            .Bind(() => cache.TrySaveRandomFile(settings.BasePath, "png"))
            .FinallyDispose(_ => cache);
    }
}