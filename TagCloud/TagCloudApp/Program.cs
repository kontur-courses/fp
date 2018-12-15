using Autofac;
using Autofac.Core;
using TagCloudCreation;
using TagCloudVisualization;

namespace TagCloudApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = new ContainerBuilder();

            container.RegisterType<TagCloudCreator>()
                     .AsSelf();

            container.RegisterType<CircularCloudLayouter>()
                     .AsSelf();

            container.RegisterTypes(typeof(WhitespaceTextReader)) 
                     .As<ITextReader>();

            container.RegisterTypes(typeof(ShortWordDrawer), typeof(BasicDrawer)).WithOrder()
                     .As<IWordDrawer>();

            container.RegisterType<CompositeDrawer>()
                     .AsSelf().WithParameter(new ResolvedParameter((i,c)=>true, (i,c)=>c.ResolveOrdered<IWordDrawer>()));

            container.RegisterType<RoundSpiralGenerator>()
                     .As<AbstractSpiralGenerator>();

            container.RegisterTypes(typeof(FixedBoringWordsRemover), typeof(LowerCaseSetter), typeof(Formatter),
                                    /*typeof(SelectedBoringWordsRemover),*/ typeof(VerbRemover), typeof(PrepositionRemover)) // можно раскомментировать,
                                                                                                                            // но тогда нужно указать файл со списком "скучных" слов
                                                                                                                           // во входных аргументах (просто как пример)
                     .As<IWordPreparer>();

            container.RegisterType<TagCloudStatsGenerator>()
                     .As<ITagCloudStatsGenerator>();
            container.RegisterType<PathValidator>()
                     .As<IPathValidator>();
            container.RegisterType<TagCloudImageCreator>()
                     .AsSelf();

            container.RegisterType<ConsoleUserInterface>()
                     .As<UserInterface>()
                     .SingleInstance();

            using (var scope = container.Build()
                                        .BeginLifetimeScope())
            {
                var ui = scope.Resolve<UserInterface>();
                ui.Run(args);
            }
        }
    }
}
