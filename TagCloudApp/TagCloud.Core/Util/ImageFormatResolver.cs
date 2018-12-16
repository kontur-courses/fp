using System;
using System.Drawing.Imaging;
using System.Linq;

namespace TagCloud.Core.Util
{
    public class ImageFormatResolver
    {
        public static Result<ImageFormat> ResolveFromFileName(string fileName)
        {
            return Result
                .Of(() => typeof(ImageFormat).GetProperties().FirstOrDefault(
                    p => fileName.EndsWith(p.Name, StringComparison.InvariantCultureIgnoreCase)))
                .Then(i => i.GetValue(i) as ImageFormat)
                .ReplaceError(err => $"Can't recognize format of file \"{fileName}\".");
        }
    }
}