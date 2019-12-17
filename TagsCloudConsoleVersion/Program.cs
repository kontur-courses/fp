using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CommandLine;
using ResultOf;
using TagsCloudGenerator.Core.Drawers;
using TagsCloudGenerator.Core.Translators;
using TagsCloudGenerator.Infrastructure;

namespace TagsCloudConsoleVersion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
            {
                new OptionsValidator().ValidateOptions(options)
                    .Then(Run)
                    .OnFail(Console.WriteLine);
            });
        }

        private static Result<None> Run(Options options)
        {
            var palette = options.ColorTheme.Palette();
            var cloudDrawer =
                new RectangleCloudDrawer(palette.BackgroundColor, new SolidBrush(palette.PrimaryColor));
            return TextToTagsTranslatorFactory.Create(options.Alpha, options.Phi)
                .Then(translator => translator.TranslateTextToTags(
                    File.ReadLines(options.InputFilename),
                    new HashSet<string>(),
                    options.FontFamily,
                    options.MinFontSize))
                .Then(tags => cloudDrawer.DrawCloud(tags.ToList()))
                .Then(bitmap => {
                    bitmap = ImageUtils.ResizeImage(bitmap, options.Width, options.Height);
                    bitmap.Save(options.OutputFilename,
                        ImageFormatUtils.GetImageFormatByExtension(options.ImageExtension));
                });
        }
    }
}