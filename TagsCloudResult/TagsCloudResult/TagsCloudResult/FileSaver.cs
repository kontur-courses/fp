using ResultOf;
using System;
using System.Drawing;
using System.IO;

namespace TagsCloudContainer
{
    public static class FileSaver
    {
        public static Result<string> SaveImage(Bitmap image, Func<Result<string>> getImageName, string saveDir, ImageFormats format)
        {
            var savePath = getImageName()
                .Then(name => @$"{saveDir}\{name}.{format}")
                .Then
                (
                    path =>
                    {
                        image.Save(path);
                        return Path.GetFullPath(path);
                    }
                );
            return savePath;
        }
    }
}
