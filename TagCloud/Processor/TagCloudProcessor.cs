using TagCloud.Analyzers;
using TagCloud.Creators;
using TagCloud.Layouters;
using TagCloud.Provider;
using TagCloud.Readers;
using TagCloud.ResultMonad;
using TagCloud.Settings;
using TagCloud.Visualizers;
using TagCloud.Writers;

namespace TagCloud.Processor
{
    public class TagCloudProcessor : ITagCloudProcessor
    {
        private readonly IFileReaderFactory readerFactory;
        private readonly ITextAnalyzer textAnalyzer;
        private readonly ITagCreator tagCreator;
        private readonly ICloudLayouter layouter;
        private readonly IVisualizer visualizer;
        private readonly ITagColoringFactory tagColoringFactory;
        private readonly IFileWriter writer;
        private readonly IProcessorSettings settings;
        private readonly IWordProvider wordProvider;

        public TagCloudProcessor(IFileReaderFactory readerFactory,
            ITextAnalyzer textAnalyzer,
            ITagCreator tagCreator,
            ICloudLayouter layouter,
            IVisualizer visualizer,
            ITagColoringFactory tagColoringFactory,
            IFileWriter writer,
            IProcessorSettings settings,
            IWordProvider wordProvider)
        {
            this.readerFactory = readerFactory;
            this.textAnalyzer = textAnalyzer;
            this.tagCreator = tagCreator;
            this.layouter = layouter;
            this.visualizer = visualizer;
            this.tagColoringFactory = tagColoringFactory;
            this.writer = writer;
            this.settings = settings;
            this.wordProvider = wordProvider;
        }

        public void Run()
        {
            readerFactory.Create(GetExtensionsFromFileName(settings.ExcludedWordsFile))
                .Then(reader => reader.ReadFile(settings.ExcludedWordsFile))
                .Then(words => wordProvider.AddWords(words))
                .Then(n => readerFactory.Create(GetExtensionsFromFileName(settings.InputFilename)))
                .Then(reader => reader.ReadFile(settings.InputFilename))
                .Then(words => textAnalyzer.Analyze(words))
                .Then(wordFrequencies => tagCreator.Create(wordFrequencies))
                .Then(tags => layouter.PutTags(tags))
                .Then(tags => visualizer.DrawCloud(tags, tagColoringFactory))
                .Then(bitmap => writer.Write(bitmap,
                    settings.OutputFilename,
                    GetExtensionsFromFileName(settings.OutputFilename),
                    settings.TargetDirectory))
                .OnFail(System.Console.WriteLine);
        }

        private static string GetExtensionsFromFileName(string filename)
        {
            var lastDotIndex = filename.LastIndexOf('.');
            return filename[(lastDotIndex + 1)..];
        }
    }
}
