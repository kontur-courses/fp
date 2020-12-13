using System.Collections.Generic;
using System.IO;
using CommandLine;
using TagCloud.ImageSaver;

namespace TagCloud.Visualizer.Console
{
    [Verb("print")]
    public class PrintCommand
    {
        private static readonly string SavePath = Path.Combine(Directory.GetCurrentDirectory(),
            "..",
            "..",
            "..",
            "..",
            $"{nameof(TagCloud)}.{nameof(Visualizer)}",
            "images");

        public static int PrintCloudAndReturnExitCode(ICloudLayouter cloudLayouter, IEnumerable<string> words,
            ImageOptions imageOptions, IImageSaver imageSaver)
        {
            var sizesResult = SizesCreator.CreateSizesArray(words, imageOptions.FontSize, imageOptions.FontName);
            var rectsResult = cloudLayouter.GetRectangles(sizesResult);
            var bitmapResult = BitmapCreator.DrawBitmap(rectsResult, imageOptions);
            imageSaver.TrySaveImage(bitmapResult, SavePath, imageOptions);
            bitmapResult.GetValueOrThrow().Dispose();
            return 0;
        }
    }
}