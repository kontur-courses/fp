using System.Drawing;
using ResultOf;
using TagCloud.Common.Options;

namespace TagCloud.Common.Saver;

public class LocalCloudSaver : ICloudSaver
{
    public SavingOptions CloudSavingOptions { get; }

    public LocalCloudSaver(SavingOptions savingOptions)
    {
        CloudSavingOptions = savingOptions;
    }

    public Result<None> SaveCloud(Bitmap bmp)
    {
        if (!Directory.Exists(CloudSavingOptions.SavePath))
        {
            return Result.Fail<None>("Saving directory doesn't exist");
        }

        bmp.Save(CloudSavingOptions.GetFullSavingPath());
        bmp.Dispose();
        return Result.Ok();
    }
}