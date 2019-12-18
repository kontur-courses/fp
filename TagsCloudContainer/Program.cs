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
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(options =>
                   {
                       var result = Work(options);

                       if (result.IsSuccess)
                           result.Value.Save(options.Image);
                       else
                           Console.WriteLine(result.Error);
                   });
        }

        private static Result<Bitmap> Work(Options o)
        {
            if (o.Size <= 0)
                return Result.Fail<Bitmap>("Font size must be a natural number");
            if (o.Height <= 0 || o.Width <= 0)
                return Result.Fail<Bitmap>("Image sizes must be positive numbers.");

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>().WithParameter("center", new Point());
            containerBuilder.RegisterType<SimpleWordPreprocessor>().As<IWordPreprocessor>();
            containerBuilder.RegisterType<SimpleWordFilter>().As<IWordFilter>();
            containerBuilder.RegisterType<SimpleWordCounter>().As<IWordCounter>();

            containerBuilder.RegisterType<SimplePalette>().As<IPalette>()
            .WithParameters(
                new Parameter[]
                {
                               new NamedParameter("font", new Font(o.Font, o.Size)),
                               new NamedParameter("painterWords", new SimplePainterWords(new SolidBrush(Color.FromName(o.Color))))
                }
                );
            containerBuilder.RegisterType<SimpleVisualizer>().As<IVisualizer>().WithParameter("imageSettings", new ImageSettings(o.Height, o.Width));
            containerBuilder.RegisterType<SimpleReader>().As<IReader>().WithParameter("path", o.File);

            containerBuilder.RegisterType<TagsCloudGenerator>().As<TagsCloudGenerator>();

            var container = containerBuilder.Build();
            var tagsCloudGenerator = container.Resolve<TagsCloudGenerator>();

            var result = tagsCloudGenerator.CreateTagCloud();
            return result;
        }
    }
}
