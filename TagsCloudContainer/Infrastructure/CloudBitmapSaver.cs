namespace TagsCloudContainer.Infrastructure;

public class CloudBitmapSaver
{
    private readonly Settings.Settings settings;

    public CloudBitmapSaver(Settings.Settings settings)
    {
        this.settings = settings;
    }

    public string SaveBitmap(Bitmap bm)
    {
        var layoutsPath = Path.Combine(Path.GetFullPath(@"..\..\..\"), "layouts");
        if (!Directory.Exists(layoutsPath))
            Directory.CreateDirectory(layoutsPath);
        var savePath = $"{layoutsPath}\\layout_{DateTime.Now:HHmmssddMM}.{settings.Format}";
        bm.Save(savePath, settings.Format);
        return savePath;
    }
}
