using System.Drawing;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult
{
    public static class ImageCreator
    {
        public static Result<None> Save(Bitmap image, ImageSetting setting)
        {
            image.Save($"{setting.Name}.{setting.Format}");
            image.Dispose();
            return Result.Ok();
        }
    }
}