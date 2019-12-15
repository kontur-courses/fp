using FailuresProcessing;
using TagsCloudGenerator.Executors;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.Generators
{
    public class TagsCloudGenerator : IGenerator
    {
        private readonly IFactory<IWordsParser> parsersFactory;
        private readonly IFactory<IWordsConverter> convertersFactory;
        private readonly IFactory<IWordsFilter> filtersFactory;
        private readonly IFactory<IWordsLayouter> layoutersFactory;
        private readonly IPainterAndSaver painterAndSaver;

        public TagsCloudGenerator(
            IFactory<IWordsParser> parsersFactory,
            IFactory<IWordsConverter> convertersFactory,
            IFactory<IWordsFilter> filtersFactory,
            IFactory<IWordsLayouter> layoutersFactory,
            IPainterAndSaver painterAndSaver)
        {
            this.parsersFactory = parsersFactory;
            this.convertersFactory = convertersFactory;
            this.filtersFactory = filtersFactory;
            this.layoutersFactory = layoutersFactory;
            this.painterAndSaver = painterAndSaver;
        }

        public Result<None> Generate(string readFromPath, string saveToPath)
        {
            return
                Result.Ok()
                .Then(none => 
                    parsersFactory.CreateSingle()
                    .Then(parser => parser.ParseFromFile(readFromPath)))
                .Then(words =>
                    convertersFactory.CreateArray()
                    .Then(converters => new PriorityExecutor<string[]>(converters).Execute(words)))
                .Then(words =>
                    filtersFactory.CreateArray()
                    .Then(filters => new PriorityExecutor<string[]>(filters).Execute(words)))
                .Then(words =>
                    layoutersFactory.CreateSingle()
                    .Then(layouter => layouter.ArrangeWords(words)))
                .Then(layoutedWords => painterAndSaver.PaintAndSave(layoutedWords, saveToPath))
                .RefineError("Tags cloud generation failed");
        }
    }
}