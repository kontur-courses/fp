using System;
using System.IO;
using TagCloud.Algorithm.SpiralBasedLayouter;
using TagCloud.Infrastructure;
using TagCloud.TextReading;
using TagCloud.Visualization;
using TagCloud.WordsProcessing;
using Autofac;
using CommandLine;
using TagCloud.Algorithm;
using TagCloud.App;
using TagCloud.Visualization.WordPainting;

namespace TagCloud
{
    public class Program
    {
        private static void RegisterOptionIndependentDependencies(ContainerBuilder builder)
        {
            builder.RegisterInstance(new PictureConfig()).As<PictureConfig>();
            builder.RegisterType<TxtTextReader>().As<ITextReader>();
            builder.RegisterType<MicrosoftWordTextReader>().As<ITextReader>();
            builder.RegisterType<TextReaderSelector>().As<ITextReaderSelector>();
            builder.RegisterType<FileInfoProvider>().As<IFileInfoProvider>();
            builder.RegisterType<WordCounter>().As<IWordCounter>();
            builder.RegisterType<WordSizeSetter>().As<IWordSizeSetter>();
            builder.RegisterType<WordProcessor>().As<IWordProcessor>();
            builder.RegisterType<WordClassBasedSelector>().As<IWordSelector>();
            builder.RegisterType<WordPainterFabric>().As<IWordPainterFabric>();
            builder.RegisterType<IndexBasedWordPainter>().As<IWordPainter>()
                .InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies); 
            builder.RegisterType<RandomColorWordPainter>().As<IWordPainter>();
            builder.RegisterType<WordClassBasedWordPainter>().As<IWordPainter>();
            builder.RegisterType<CircularCloudLayouter>().As<ITagCloudLayouter>();
            builder.RegisterType<ArchimedeanSpiral>().As<ISpiral>();
            builder.RegisterType<PngImageFormat>().As<IImageFormat>();
            builder.RegisterType<TagCloudGenerator>().As<ITagCloudGenerator>();
            builder.RegisterType<TagCloudElementsPreparer>().As<ITagCloudElementsPreparer>()
                .InstancePerLifetimeScope();
            builder.RegisterType<WordDrawer>().As<ITagCloudElementDrawer>();
            builder.RegisterType<Application>().AsSelf();
        }

        private static void Execute(Options options)
        {
            var projectsDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var myStemPath = $"{projectsDirectory}/mystem.exe";

            var builder = new ContainerBuilder();
            RegisterOptionIndependentDependencies(builder);
            builder.RegisterInstance(new MyStemBasedWordClassIdentifier(myStemPath)).As<IWordClassIdentifier>();

            builder.Register(c =>
                    new ConsoleSettingsProvider(options, c.Resolve<PictureConfig>(), c.Resolve<IFileInfoProvider>()))
                .As<ISettingsProvider>().InstancePerLifetimeScope();
           
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<Application>();
                var result = app.Run();
                if (!result.IsSuccess)
                    Console.WriteLine(result.Error);
            }
        }


        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Execute)
                .WithNotParsed(errors => Console.WriteLine(string.Join("\\n", errors)));
        }
    }
}
