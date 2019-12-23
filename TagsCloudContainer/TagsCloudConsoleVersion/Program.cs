using System;
using Autofac;
using CloudDrawing;
using CloudLayouter;
using CloudLayouter.Spiral;
using CommandLine;
using ResultOf;
using TagsCloudContainer.PreprocessingWords;
using TagsCloudContainer.ProcessingWords;
using TagsCloudContainer.Reader;

namespace TagsCloudConsoleVersion
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(option =>
            {
                GetProcessor(option.UseSqueezedAlgorithm)
                    .Then(() => ImageSettings.GetImageSettings(option.ColorBackground, option.Width, option.Height))
                    .Then(() => WordDrawSettings.GetWordDrawSettings(option.FamyilyNameFont, option.ColorBrush, option.HaveDelineation))
                    .Then(tuple =>
                    {
                        var (processor, imageSettings, wordDrawSettings) = tuple;
                        return processor.Run(option.PathToFile, option.PathSaveFile, imageSettings, wordDrawSettings);
                    })
                    .OnFail(Console.WriteLine);
            });
        }

        private static Result<IProcessor> GetProcessor(bool useSqueezedAlgorithm)
        {
            return new ContainerBuilder().AsResult()
                .Apply(builder => builder.RegisterType<CircularSpiral>().As<ISpiral>())
                .Apply(builder => builder.RegisterType<CircularCloudDrawing>().As<ICircularCloudDrawing>())
                .Apply(builder => builder.RegisterType<ReaderFromTxt>().As<IReaderFromFile>())
                .Apply(builder => builder.RegisterType<MyStemUtility>().As<IPreprocessingWords>())
                .Apply(builder => builder.RegisterType<CreateProcess>().As<ICreateProcess>())
                .Apply(builder => builder.RegisterType<Processor>().As<IProcessor>())
                .Apply(builder =>
                {
                    if (useSqueezedAlgorithm)
                        builder.RegisterType<SqueezedCircularCloudLayouter>().As<ICloudLayouter>();
                    else
                        builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
                })
                .Then(builder => builder.Build().Resolve<IProcessor>())
                .RefineError("Ошибка при сборке графа зависимостей");
        }
    }
}