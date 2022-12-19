using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Interfaces;
using TagsCloud.TextWorkers;
using TagsCloud.Validators;

namespace TagsCloud
{
    public class ContainerBuilder
    {
        public static ServiceProvider GetNewTagCloudServices(int width, int height)
        {
            var centerPoint = new Point((int)(width / 2), (int)(height / 2));

            var services = new ServiceCollection();

            services.AddSingleton<ISpiral>(x => new Spiral(centerPoint));
            services.AddSingleton<Spiral, Spiral>();

            services.AddSingleton<IRectangleComposer, RectangleComposer>();
            services.AddSingleton<RectangleComposer, RectangleComposer>();

            services.AddSingleton<ICloudLayouter, CircularCloudLayouter>();
            services.AddSingleton<CircularCloudLayouter, CircularCloudLayouter>();

            services.AddSingleton<ISettingsValidator, SettingsValidator>();
            services.AddSingleton<IPrintSettings>(x =>
            {
                var settings = new PrintSettings(x.GetService<ISettingsValidator>());
                settings.SetFont("Consolas", 64);
                settings.SetCentralPen(Color.White, 8);
                settings.SetSurroundPen(Color.FromArgb(249, 100, 0), 4);
                settings.SetBackgroudColor(Color.FromArgb(0, 34, 43));
                settings.SetPictureSize(width, height);
                return settings;
            });

            services.AddSingleton<IBitmapper, Bitmapper>();

            services.AddSingleton<ITextPartsToExclude, TextPartsToExclude>();

            services.AddSingleton<IComparer<int>, ReverseComparer<int>>();
            services.AddSingleton<IComparer<double>, ReverseComparer<double>>();

            services.AddSingleton<IMorphsFilter, MorphsFilter>();
            services.AddSingleton<IWordsFrequencyAnalyzer, WordsFrequencyAnalyzer>();
            services.AddSingleton<IWordsRectanglesScaler, WordsRectanglesScaler>();

            services.AddSingleton<IMorphsParser, MorphsParser>();
            services.AddSingleton<INormalFormParser, NormalFormParser>();
            services.AddSingleton<ITextSplitter, TextSplitter>();
            services.AddSingleton<IFileValidator, FileValidator>();
            services.AddSingleton<IBitmapValidator, BitmapValidator>();
            services.AddSingleton<IMorphsValidator, MorphsValidator>();

            services.AddSingleton<ITagCloud, TagCloud>();
            services.AddSingleton<TagCloud, TagCloud>();

            services.AddSingleton<IRecognizer<string>, Recognizer>();
            services.AddSingleton<IClientValidator, ClientValidator>();
            services.AddSingleton<IClient>(x => new ConsoleClient(
                x.GetService<IRecognizer<string>>(),
                x.GetService<IClientValidator>(),
                @"..\..\..\..\"));

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
