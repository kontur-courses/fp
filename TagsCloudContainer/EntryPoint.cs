using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using TagsCloudContainer.Drawing;
using TagsCloudContainer.Input;
using TagsCloudContainer.Layout;
using TagsCloudContainer.Output;
using TagsCloudContainer.Processing;
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
                Component.For<IFileReader>().ImplementedBy<TxtReader>(),
                Component.For<IParser>().ImplementedBy<WordParser>(),
                Component.For<IRectangleLayout>().ImplementedBy<CircularCloudLayout>(),
                Component.For<IWordLayout>().ImplementedBy<WordLayout>(),
                Component.For<IDrawer>().ImplementedBy<ImageDrawer>(),
                Component.For<IWriter>().ImplementedBy<FileWriter>(),
                Component.For<IUi>().ImplementedBy<ConsoleUi>(),
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