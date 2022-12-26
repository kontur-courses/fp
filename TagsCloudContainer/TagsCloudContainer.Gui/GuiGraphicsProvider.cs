using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Gui;

public class GuiGraphicsProvider : IGraphicsProvider
{
    private readonly IImageListProvider imageListProvider;
    private readonly GuiGraphicsProviderSettings settings;
    private Bitmap? bitmap;
    private Graphics? graphics;

    public GuiGraphicsProvider(
        IImageListProvider imageListProvider,
        GuiGraphicsProviderSettings settings)
    {
        this.imageListProvider = imageListProvider;
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
            .BindIf(settings.Save, () => cache.TrySaveRandomFile(settings.SavePath, "png"))
            .Bind(() => imageListProvider.AddImageBits(cache.ToArray()))
            .AnyWayDispose(_ => cache);
    }
}