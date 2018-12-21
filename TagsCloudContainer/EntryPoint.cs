using System;
using System.Drawing;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using TagsCloudContainer.Drawing;
using TagsCloudContainer.Input;
using TagsCloudContainer.Layout;
using TagsCloudContainer.Output;
using TagsCloudContainer.Processing;
using TagsCloudContainer.Processing.Converting;
using TagsCloudContainer.Processing.Filtering;
using TagsCloudContainer.Settings;
using TagsCloudContainer.Ui;

namespace TagsCloudContainer
{
    public static class EntryPoint
    {
        private static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

            container.Register(
                Component.For<IUi>().ImplementedBy<ConsoleUi>(),
                Component.For<IFileReader>().ImplementedBy<TxtReader>(),

                Component.For<IWordFilter>().ImplementedBy<DefaultFilter>(),
                Component.For<IWordFilter>().ImplementedBy<CommonWordsFilter>(),
                Component.For<IWordFilter>().ImplementedBy<BlackListFilter>().DependsOn(
                    Dependency.OnValue("wordsToFilter", new[] {"плохой", "ужасный"})),

                Component.For<IWordConverter>().ImplementedBy<InitialFormConverter>(),
                Component.For<IWordConverter>().ImplementedBy<DefaultConverter>(),

                Component.For<IParser>().ImplementedBy<WordParser>(),

                Component.For<IRectangleLayout>().ImplementedBy<CircularCloudLayout>().DependsOn(
                    Dependency.OnValue("center", new Point(512, 512)),
                    Dependency.OnValue("size", new Size(1024, 1024))),

                Component.For<IWordLayout>().ImplementedBy<WordLayout>(),
                Component.For<IDrawer>().ImplementedBy<ImageDrawer>(),
                Component.For<IWriter>().ImplementedBy<FileWriter>(),
                Component.For<Transformer>()
            );


            var ui = container.Resolve<IUi>();
            var options = ui.RetrievePaths(args);

            if (options == null)
                return;

            var imageSettings = SettingsCreator.CreateImageSettings(options);
            if (imageSettings.IsFailure)
            {
                Console.WriteLine(imageSettings.Error);
                return;
            }

            container.Register(
                Component.For<ImageSettings>().Instance(imageSettings.Value),
                Component.For<ParserSettings>().Instance(SettingsCreator.CreateParserSettings(options))
                );
            container.Resolve<Transformer>().TransformWords(options);
        }
    }
}