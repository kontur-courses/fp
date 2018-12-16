using System;
using System.Drawing.Imaging;
using System.Linq;

namespace TagCloud.Core.Util
{
    public class ImageFormatResolver
    {
        public static Result<ImageFormat> TryResolveFromFileName(string fileName)
        {
            return Result
                .Of(() => typeof(ImageFormat).GetProperties().FirstOrDefault(
                    p => fileName.EndsWith(p.Name, StringComparison.InvariantCultureIgnoreCase)))
                .Then(i => i.GetValue(i) as ImageFormat)
                .RefineError($"Can't recognize format of file \"{fileName}\".\n");
        }
    }
}