using TagsCloud.Factory;
using TagsCloud.ResultOf;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloud.Layouter.Factory
{
    public class RectanglesLayoutersFactory : ServiceFactory<IRectanglesLayouter>
    {
        private readonly WordConfig wordsConfig;

        public RectanglesLayoutersFactory(WordConfig wordsConfig)
        {
            this.wordsConfig = wordsConfig;
        }

        public override Result<IRectanglesLayouter> Create() =>
             Result.Of(() => services[wordsConfig.LayouterName](),
                $"This layouter {wordsConfig.LayouterName ?? "null"} not supported");
    }
}
