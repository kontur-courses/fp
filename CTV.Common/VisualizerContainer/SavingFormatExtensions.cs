using System;
using CTV.Common.ImageSavers;

namespace CTV.Common.VisualizerContainer
{
    public static class SavingFormatExtensions
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