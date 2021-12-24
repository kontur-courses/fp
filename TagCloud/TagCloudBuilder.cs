using System.Collections.Generic;
using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using TagCloud.file_readers;
using TagCloud.layouter;
using TagCloud.repositories;
using TagCloud.selectors;
using TagCloud.settings;
using TagCloud.visual;

namespace TagCloud
{
    public class TagCloudBuilder
    {
        private readonly ServiceCollection container;
        private IDrawSettings drawSettings;
        private ITagSettings tagSettings;
        private string? input;
        private string? output;

        private TagCloudBuilder()
        {
            tagSettings = new TagSettings(FontFamily.GenericSansSerif, 15);
            drawSettings = new DrawSettings(
                new List<Color> { Color.Magenta, Color.Aqua, Color.Azure },
                Color.Black,
                new Size(1500, 1500)
            );
            container = new ServiceCollection();
            container.AddSingleton<ICloudVisualizer, TagCloudVisualizer>()
                .AddSingleton<IWordHelper, WordHelper>()
                .AddSingleton<IFilter<string>>(new WordFilter(
                        new List<IChecker<string>>
                            { new BoringChecker() }
                    )
                )
                .AddSingleton<IConverter<IEnumerable<string>>>(
                    new WordConverter(new List<IConverter<string>>
                        { new ToLowerCaseConverter() }
                    )
                )
                .AddSingleton<ISaver<Image>, TagVisualizationSaver>()
                .AddSingleton<ICloudLayouter, CircularCloudLayouter>()
                .AddSingleton<IFileReader, FileReader>();
        }

        public static TagCloudBuilder Create() => new TagCloudBuilder();

        public TagCloudBuilder SetInputFile(string filename)
        {
            input = filename;
            return this;
        }

        public TagCloudBuilder SetOutputFile(string filename)
        {
            output = filename;
            return this;
        }

        public TagCloudBuilder SetDrawSettings(IDrawSettings settings)
        {
            drawSettings = settings;
            return this;
        }

        public TagCloudBuilder SetTagSettings(ITagSettings settings)
        {
            tagSettings = settings;
            return this;
        }

        public TagCloudBuilder Build()
        {
            container
                .AddSingleton(tagSettings)
                .AddSingleton(drawSettings);
            return this;
        }

        public Result Run()
        {
            var scope = container.BuildServiceProvider().CreateScope();
            return scope.ServiceProvider.GetRequiredService<IFileReader>().GetWords(input)
                .Then(scope.ServiceProvider.GetRequiredService<ICloudVisualizer>().InitializeCloud)
                .Then(v => v.GetImage(drawSettings))
                .Then(i => scope.ServiceProvider.GetRequiredService<ISaver<Image>>().Save(i, output!));
        }
    }
}