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

                Component.For<IParser>().ImplementedBy<WordParser>(),
                Component.For<ImageSettings>().DependsOn(
                    Dependency.OnValue("size", new Size(1024, 1024)),
                    Dependency.OnValue("textFont", new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular)),
                    Dependency.OnValue("maxFontSize", 100),
                    Dependency.OnValue("minFontSize", 10),
                    Dependency.OnValue("backgroundColor", Color.White),
                    Dependency.OnValue("textColor", Color.Red)),

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
            container.Resolve<Transformer>().TransformWords(options.TextFile, options.ImageFile);
        }
    }
}