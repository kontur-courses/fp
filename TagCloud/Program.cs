using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using TagCloud.Actions;
using TagCloud.Factories;
using TagCloud.IServices;

namespace TagCloud
{
    public class Program
    {
        public static void Main()
        {
            var container = GetContainer();
            var client = container.Resolve<IClient>();
            client.Start();
        }

        public static WindsorContainer GetContainer()
        {
            var container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel, true));
            container.Register(Component.For<IPaletteDictionaryFactory>().ImplementedBy<PaletteDictionaryFactory>()
                .LifestyleSingleton());
            container.Register(Component.For<IFontSettingsFactory>().ImplementedBy<FontSettingsFactory>()
                .LifestyleSingleton());
            container.Register(Component.For<IBoringWordsFactory>().ImplementedBy<BoringWordsFactory>());
            container.Register(Component.For<IWordsToTagsParser>().ImplementedBy<WordsToTagsParser>());
            container.Register(Component.For<IWordsHandler>().ImplementedBy<WordsHandler>());
            container.Register(Component.For<ITagCollectionFactory>().ImplementedBy<TagCollectionFactory>()
                .LifestyleSingleton());
            container.Register(Component.For<IAlgorithm>().ImplementedBy<ArchimedianSprial>());
            container.Register(Component.For<ICircularCloudLayouter>().ImplementedBy<CircularCloudLayouter>());
            container.Register(Component.For<ICloud>().ImplementedBy<Cloud>());
            container.Register(Component.For<ICloudVisualization>().ImplementedBy<CloudVisualization>());
            container.Register(Component.For<IPaletteNamesFactory>().ImplementedBy<PaletteNamesFactory>()
                .LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<NewImageConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<SaveImageConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<ShowImageConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<ExitConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<GetFontNameConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<GetHeightConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<GetPaletteNameConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<GetWidthConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<GetPathConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IConsoleAction>().ImplementedBy<HelpConsoleAction>().LifestyleSingleton());
            container.Register(Component.For<IClient>().ImplementedBy<Client>());
            return container;
        }
    }
}