using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Autofac;
using Autofac.Core;
using CommandLine;
using TagCloud.Analyzers;
using TagCloud.Creators;
using TagCloud.Layouters;
using TagCloud.Readers;
using TagCloud.Visualizers;
using TagCloud.Writers;

namespace TagCloud.UI.Console
{
    public class ConsoleUI : IUserInterface
    {
        //private readonly IFileReaderFactory readerFactory;
        //private readonly ITextAnalyzer textAnalyzer;
        //private readonly BoringWordsFilter boringWordsFilter;
        //private readonly ICloudLayouter layouter;
        //private readonly IVisualizer visualizer;
        //private readonly IFileWriter writer;
        //private readonly ITagCreator tagCreator;
        //private readonly ITagColoringFactory tagColoringFactory;

        //public ConsoleUI(IFileReaderFactory readerFactory,
        //    ITextAnalyzer textAnalyzer,
        //    BoringWordsFilter boringWordsFilter,
        //    ICloudLayouter layouter,
        //    IVisualizer visualizer,
        //    IFileWriter writer, 
        //    ITagCreator tagCreator,
        //    ITagColoringFactory tagColoringFactory)
        //{
        //    this.readerFactory = readerFactory;
        //    this.textAnalyzer = textAnalyzer;
        //    this.layouter = layouter;
        //    this.visualizer = visualizer;
        //    this.writer = writer;
        //    this.tagCreator = tagCreator;
        //    this.tagColoringFactory = tagColoringFactory;
        //    this.boringWordsFilter = boringWordsFilter;
        //}

        public void Run(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleParseError);
        }

        private void Run(Options options)
        {
            DependencyConfigurator
                .GetConfiguredContainer(options)
                .AsResult()
                .Then(container => Execute(container, options))
                .OnFail(System.Console.WriteLine);
        }

        private void Execute(IContainer container, Options options)
        {
            //Result.Of(() => GetDrawingSettings(options));
            //var center = new Point(options.Width / 2, options.Height / 2);
            //var drawingSettings = Result.Of(() => GetDrawingSettings(options)); //тут на самом деле хз где его вызывать.
            //using (drawingSettings.Value)
            //{
            //    var fileExtension = Result.Of(() => GetExtensionsFromFileName(options.InputFilename));
            //    var outputExtension = Result.Of(() => GetExtensionsFromFileName(options.OutputFilename)); //можно вынести внутрь
            //    var boringWordsFileExtension = Result.Of(() => GetExtensionsFromFileName(options.ExcludedWordsFile)); //тоже
            //    var tagColoring = tagColoringFactory.Create(options.TagColoring, drawingSettings.Value.PenColors) //хз че делать
            //        .Value;

            //    var reader = fileExtension.Then(e => readerFactory.Create(e))
            //        .Then(reader => reader.ReadFile(options.InputFilename))
            //        .Then(words => boringWordsFileExtension.Then(e => readerFactory.Create(e))
            //            .Then(reader => reader.ReadFile(options.ExcludedWordsFile))
            //            .Then(wordsToExclude => boringWordsFilter.AddWords(wordsToExclude))
            //            .Then(n => textAnalyzer.Analyze(words)))
            //        .Then(wordFrequencies => drawingSettings.Then(settings => tagCreatorFactory
            //            .Create(settings.Font)
            //            .Create(wordFrequencies)))
            //        .Then(tags => layouterFactory
            //            .Create(center)
            //            .PutTags(tags))
            //        .Then(tags => visualizer.DrawCloud(tags, drawingSettings.Value, tagColoring))
            //        .Then(bitmap => writer.Write(bitmap, options.OutputFilename, outputExtension.Value))
            //        .Then(n => drawingSettings.Value.Dispose())
            //        .OnFail(System.Console.WriteLine);

                //var text = reader.ReadFile(options.InputFilename);
                //var wordsToExclude = readerFactory.Create(boringWordsFileExtension)
                //    .ReadFile(options.ExcludedWordsFile).ToHashSet();

                //boringWordsFilter.AddWords(wordsToExclude);
                //var wordFrequencies = textAnalyzer.Analyze(text);

                //var tags = tagCreatorFactory
                //    .Create(drawingSettings.Font)
                //    .Create(wordFrequencies);


                //var placedTags = layouterFactory.Create(center).PutTags(tags);
                //using (drawingSettings)
                //{
                //    var tagColoringAlgorithm =
                //        tagColoringFactory.Create(options.TagColoring, drawingSettings.PenColors);
                //    var image = visualizer.DrawCloud(placedTags, drawingSettings, tagColoringAlgorithm);
                //    writer.Write(image, options.OutputFilename, outputExtension);
                //}
            //}
            //получился нечитаемый кал. как должно быть по идее
            var readerFactory = container.Resolve<IFileReaderFactory>();
            var boringWordsFilter = container.Resolve<BoringWordsFilter>();
            var textAnalyzer = container.Resolve<ITextAnalyzer>();
            var tagCreator = container.Resolve<ITagCreator>();
            var layouter = container.Resolve<ICloudLayouter>();
            var visualizer = container.Resolve<IVisualizer>();
            var tagColoringFactory = container.Resolve<ITagColoringFactory>();
            var writer = container.Resolve<IFileWriter>();
            //readerFactory.ToString();
            //boringWordsFilter.ToString();
            //textAnalyzer.ToString();
            //tagCreator.ToString();
            //layouter.ToString();
            //visualizer.ToString();
            //tagColoringFactory.ToString();
            //writer.ToString();

            //var boringWordsReader = readerFactory.Create(GetExtensionsFromFileName(options.ExcludedWordsFile));
            //var bw = boringWordsReader.Value.ReadFile(options.ExcludedWordsFile);
            //boringWordsFilter.AddWords(bw.Value);
            //var r = readerFactory.Create(GetExtensionsFromFileName(options.InputFilename));
            //var ws = r.Value.ReadFile(options.InputFilename);
            //var wf = textAnalyzer.Analyze(ws.Value);
            //var t = tagCreator.Create(wf);
            //var pt = layouter.PutTags(t);
            //var b = visualizer.DrawCloud(pt.Value, tagColoringFactory.Create(op));

            readerFactory.Create(GetExtensionsFromFileName(options.ExcludedWordsFile))
                .Then(reader => reader.ReadFile(options.ExcludedWordsFile))
                .Then(words => boringWordsFilter.AddWords(words))
                .Then(n => readerFactory.Create(GetExtensionsFromFileName(options.InputFilename))) //обрезать экстеншн это обязанность фабрики.
                .Then(reader => reader.ReadFile(options.InputFilename)) //как теперь прочитать скучные слова.
                .Then(words => textAnalyzer.Analyze(words)) //между этим и предыдущим пунктом, скучные слова должны пополниться.
                .Then(wordFrequencies => tagCreator.Create(wordFrequencies)) //вот здесь надо как-то фонт передать. из вариков, где-то в депенсбилдере их прям делать.
                .Then(tags => layouter.PutTags(tags)) //здесь надо центр передать
                .Then(tags => visualizer.DrawCloud(tags, tagColoringFactory)) //эта строчка отвратительная
                .Then(bitmap => writer.Write(bitmap, options.OutputFilename))
                .OnFail(System.Console.WriteLine);
        }

        private static string GetExtensionsFromFileName(string filename)
        {
            var lastDotIndex = filename.LastIndexOf('.');
            return filename[(lastDotIndex + 1)..];
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                if (error.StopsProcessing)
                    throw new ArgumentException($"Wrong command-line arguments. {error}");
            }
        }
    }
}
