using System.Drawing;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult
{
    public static class ImageCreator
    {
        public static Result<None> Save(Bitmap image, AppSettings setting)
        {
            image.Save($"{setting.ImageSetting.Name}.{setting.ImageSetting.Format}");
            image.Dispose();
            return Result.Ok();
        }
    }
}