using System;

namespace CTV.Common.ImageSavers
{
    public static class ImageSaverFactory
    {
        public static IImageSaver ToImageSaver(this SavingFormat format)
            => format switch
            {
                SavingFormat.Png => new PngSaver(),
                SavingFormat.Jpeg => new JpegSaver(),
                SavingFormat.Jpg => new JpegSaver(),
                SavingFormat.Bmp => new BmpSaver(),
                _ => throw new InvalidOperationException($"Can not find image saver for {format}")
            };
    }
}