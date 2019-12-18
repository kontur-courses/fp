using System.Drawing;
using Autofac;
using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.WordCounters;
using TagsCloudContainer.Palettes;
using TagsCloudContainer.Visualizers;
using CommandLine;
using TagsCloudContainer.WordPreprocessors;
using TagsCloudContainer.WordFilters;
using Autofac.Core;
using TagsCloudContainer.Readers;
using TagsCloudContainer.TokensAndSettings;
using TagsCloudContainer.TagsCloudGenerators;
using TagsCloudContainer.PaintersWords;
using System;

namespace TagsCloudContainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    var result = Work(options);
                    
                    if (result.IsSuccess)
                        result.GetValueOrThrow().Save(options.Image);
                    else
                        Console.WriteLine(result.Error);
                });
        }

        private static ContainerBuilder CreateContainerBuilder(Options options)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>().WithParameter("center", new Point());
            containerBuilder.RegisterType<SimpleWordPreprocessor>().As<IWordPreprocessor>();
            containerBuilder.RegisterType<SimpleWordFilter>().As<IWordFilter>();
            containerBuilder.RegisterType<SimpleWordCounter>().As<IWordCounter>();

            containerBuilder.RegisterType<SimplePalette>().As<IPalette>()
            .WithParameters(
                new Parameter[]
                {
                    new NamedParameter("font", new Font(options.Font, options.Size)),
                    new NamedParameter("painterWords", new SimplePainterWords(new SolidBrush(Color.FromName(options.Color))))
                }
                );
            containerBuilder.RegisterType<SimpleVisualizer>()
                .As<IVisualizer>()
                .WithParameter("imageSettings", new ImageSettings(options.Height, options.Width));
            containerBuilder.RegisterType<SimpleReader>().As<IReader>().WithParameter("path", options.File);

            containerBuilder.RegisterType<TagsCloudGenerator>().As<TagsCloudGenerator>();

            return containerBuilder;
        }

        private static Result<Bitmap> Work(Options options)
        {
            if (options.Size <= 0)
                return Result.Fail<Bitmap>("Font size must be a natural number");
            if (options.Height <= 0 || options.Width <= 0)
                return Result.Fail<Bitmap>("Image sizes must be positive numbers.");

            var containerBuilder = CreateContainerBuilder(options);

            var container = containerBuilder.Build();
            var tagsCloudGenerator = container.Resolve<TagsCloudGenerator>();

            var result = tagsCloudGenerator.CreateTagCloud();
            return result;
        }
    }
}
