using Autofac;
using CommandLine;
using TagCloudContainer.FileReaders;
using TagCloudContainer.FileSavers;
using TagCloudContainer.LayouterAlgorithms;
using TagCloudContainer.UI;
using TagCloudContainer.WordsColoringAlgorithms;

namespace TagCloudContainer
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            var app = new ConsoleUiSettings();

            var parsedArguments = app.GetType() == new ConsoleUiSettings().GetType()
                ? Parser.Default.ParseArguments<ConsoleUiSettings>(args).Value
                : app;
            RegisterDependencies(builder, parsedArguments);
            var container = builder.Build();
            var configuration = new Configuration(parsedArguments, container.Resolve<WordsColoringFactory>(),
                container.Resolve<FileSaverFactory>(),
                container.Resolve<FileReaderFactory>(), container.Resolve<LayouterFactory>());
            CloudDrawer.DrawWords(configuration, parsedArguments);
        }

        private static void RegisterDependencies(ContainerBuilder builder, IUi parsedArguments)
        {
            builder.RegisterInstance(new FileSaverFactory(() => parsedArguments)).As<FileSaverFactory>();
            builder.RegisterInstance(new FileReaderFactory(() => parsedArguments)).As<FileReaderFactory>();
            builder.RegisterInstance(new WordsColoringFactory(() => parsedArguments)).As<WordsColoringFactory>();
            builder.RegisterInstance(new LayouterFactory(() => parsedArguments)).As<LayouterFactory>();
            builder.RegisterInstance(parsedArguments).As<IUi>();
            builder.RegisterType<Spiral>().AsSelf();
        }
    }
}