using System.Collections.Generic;
using TagsCloudGenerator.CloudLayouter;
using TagsCloudGenerator.WordsHandler;

namespace TagsCloudGenerator
{
    public class CloudGenerator : ICloudGenerator
    {
        private readonly IWordHandler handler;
        private readonly ICloudLayouter layouter;

        public CloudGenerator(IWordHandler handler, ICloudLayouter layouter)
        {
            this.handler = handler;
            this.layouter = layouter;
        }

        public Result<Cloud> Generate(Dictionary<string, int> wordsToCount)
        {
            return handler.GetValidWords(wordsToCount)
                .Then(layouter.LayoutWords)
                .RefineError("Generator error");
        }
    }
}