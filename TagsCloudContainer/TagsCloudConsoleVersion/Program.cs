using System;
using System.Drawing;
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
            var builder = new ContainerBuilder();
            builder.RegisterType<CircularSpiral>().As<ISpiral>();
            builder.RegisterType<CircularCloudDrawing>().As<ICircularCloudDrawing>();
            builder.RegisterType<ReaderFromTxt>().As<IReaderFromFile>();
            builder.RegisterType<MyStemUtility>().As<IPreprocessingWords>();
            builder.RegisterType<CreateProcess>().As<ICreateProcess>();
            builder.RegisterType<Processor>().As<IProcessor>();

            Parser.Default.ParseArguments<Options>(args).WithParsed(option =>
            {
                var processor = builder.AsResult()
                    .Apply(b =>
                    {
                        if (option.UseSqueezedAlgorithm)
                            b.RegisterType<SqueezedCircularCloudLayouter>().As<ICloudLayouter>();
                        else
                            b.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
                    })
                    .Then(b => b.Build().Resolve<IProcessor>())
                    .RefineError("Ошибка при сборке графа зависимостей");
                if (!processor.IsSuccess)
                {
                    Console.WriteLine(processor.Error);
                    return;
                }

                var imageSettings = ImageSettings.GetImageSettings(option.ColorBackground, option.Width, option.Height);
                if (!imageSettings.IsSuccess)
                {
                    Console.WriteLine(imageSettings.Error);
                    return;
                }

                var wordDrawSettings = WordDrawSettings.GetWordDrawSettings(option.FamyilyNameFont, option.ColorBrush, option.HaveDelineation);
                if (!wordDrawSettings.IsSuccess)
                {
                    Console.WriteLine(wordDrawSettings.Error);
                    return;
                }

                var result = processor.GetValueOrThrow().Run(option.PathToFile, option.PathSaveFile,
                    imageSettings.GetValueOrThrow(), wordDrawSettings.GetValueOrThrow());
                if (!result.IsSuccess)
                    Console.WriteLine(result.Error);
            });
        }
    }
}