using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using TagsCloud.CloudLayouters;
using TagsCloud.ColorSchemes;
using TagsCloud.ErrorHandling;
using TagsCloud.Splitters;
using TagsCloud.SupportedTypes;

namespace TagsCloud
{
    public static class TypesCollector
    {
        private static readonly Dictionary<GenerationsAlgorithm, Type> SupportedLayouters =
            new Dictionary<GenerationsAlgorithm, Type>
            {
                {GenerationsAlgorithm.CircularCloud, typeof(CircularCloudLayouter)},
                {GenerationsAlgorithm.MiddleCloud, typeof(MiddleRectangleCloudLayouter)}
            };

        private static readonly Dictionary<TextSplitter, Type> SupportedSplitters = new Dictionary<TextSplitter, Type>
        {
            {TextSplitter.Line, typeof(SplitterByLine)},
            {TextSplitter.WhiteSpace, typeof(SplitterByWhiteSpace)}
        };

        private static readonly Dictionary<string, ImageFormat> SupportedImageFormat =
            new Dictionary<string, ImageFormat>
            {
                {".png", ImageFormat.Png},
                {".bmp", ImageFormat.Bmp},
                {".jpeg", ImageFormat.Jpeg}
            };

        private static readonly Dictionary<SupportedTypes.ColorSchemes, Type> SupportedColorScheme =
            new Dictionary<SupportedTypes.ColorSchemes, Type>
            {
                {SupportedTypes.ColorSchemes.RandomColor, typeof(RandomColorScheme)},
                {SupportedTypes.ColorSchemes.RedGreenBlueScheme, typeof(RedGreenBlueScheme)}
            };

        public static Result<Type> GetTypeGenerationLayoutersByName(GenerationsAlgorithm layouterName)
        {
            return SupportedLayouters.TryGetValue(layouterName, out var layouterType)
                ? layouterType.AsResult()
                : Result.Fail<Type>($"Unknown generation algorithm {layouterName}.");
            ;
        }

        public static Result<Type> GetTypeSplitterByName(TextSplitter splitterName)
        {
            return SupportedSplitters.TryGetValue(splitterName, out var splitterType)
                ? splitterType.AsResult()
                : Result.Fail<Type>($"Unsupported split format {splitterName}.");
        }

        public static Result<ImageFormat> GetFormatFromPathSaveFile(string path)
        {
            var extension = Path.GetExtension(path);
            return SupportedImageFormat.TryGetValue(extension, out var imageFormat)
                ? imageFormat.AsResult()
                : Result.Fail<ImageFormat>($"Unsupported image format {extension}.");
        }

        public static Result<Type> GetColorSchemeByName(SupportedTypes.ColorSchemes schemeName)
        {
            return SupportedColorScheme.TryGetValue(schemeName, out var colorScheme)
                ? colorScheme.AsResult()
                : Result.Fail<Type>($"Unsupported color scheme {schemeName}.");
        }
    }
}