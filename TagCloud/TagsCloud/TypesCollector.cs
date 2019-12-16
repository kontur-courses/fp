using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using TagsCloud.CloudLayouters;
using TagsCloud.ColorSchemes;
using TagsCloud.Spliters;
using TagsCloud.SupportedTypes;
using TagsCloud.ErrorHandling;

namespace TagsCloud
{
    public static class TypesCollector
    {
        private static Dictionary<GenerationsAlghoritm, Type> Layouters = new Dictionary<GenerationsAlghoritm, Type>
        {
            {GenerationsAlghoritm.CircularCloud, typeof(CircularCloudLayouter)},
            {GenerationsAlghoritm.MiddleCloud, typeof(MiddleRectangleCloudLayouter) }
        };

        private static Dictionary<TextSpliter, Type> Splitter = new Dictionary<TextSpliter, Type>
        {
            {TextSpliter.Line, typeof(SpliterByLine) },
            {TextSpliter.WhiteSpace, typeof(SpliterByWhiteSpace) }
        };

        private static Dictionary<string, ImageFormat> supportedImageFormat = new Dictionary<string, ImageFormat>
        {
            {".png", ImageFormat.Png },
            {".bmp", ImageFormat.Bmp },
            {".jpeg", ImageFormat.Jpeg }
        };

        private static Dictionary<SupportedTypes.ColorSchemes, Type> supportedColodScheme = new Dictionary<SupportedTypes.ColorSchemes, Type>
        {
            { SupportedTypes.ColorSchemes.RandomColor, typeof(RandomColorScheme) },
            { SupportedTypes.ColorSchemes.RedGreenBlueScheme, typeof(RedGreenBlueScheme) }
        };

        public static Result<Type> GetTypeGeneationLayoutersByName(GenerationsAlghoritm layouterName)
        {
            Type type;
            Layouters.TryGetValue(layouterName, out type);
            if (type == null)
                return Result.Fail<Type>($"Unknown generation algoritm {layouterName}.");
            return type;
        }

        public static Result<Type> GetTypeSpliterByName(TextSpliter spliterName)
        {
            Type type;
            Splitter.TryGetValue(spliterName, out type);
            if (type == null)
                return Result.Fail<Type>($"Unsupported split format {spliterName}.");
            return type.AsResult();
        }

        public static Result<ImageFormat> GetFormatFromPathSaveFile(string path)
        {
            var extension = Path.GetExtension(path);
            ImageFormat imageFormat;
            supportedImageFormat.TryGetValue(extension, out imageFormat);
            if (imageFormat == null)
                return Result.Fail<ImageFormat>($"Unsupported image format {extension}.");
            return imageFormat.AsResult();
        }

        public static Result<Type> GetColorSchemeByName(SupportedTypes.ColorSchemes schemeName)
        {
            Type colorScheme;
            supportedColodScheme.TryGetValue(schemeName, out colorScheme);
            if (colorScheme == null)
                return Result.Fail<Type>($"Unsupported color scheme {schemeName}.");
            return colorScheme.AsResult();
        }
    }
}
