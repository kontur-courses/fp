using System.Collections.Generic;
using System.Linq;
using TagCloud.ExceptionHandler;
using TagCloud.Interfaces;
using TagCloud.TagCloudVisualization.Analyzer;
using TagCloud.TagCloudVisualization.Layouter;

namespace TagCloud.Words
{
    public class TagGenerator : ITagGenerator
    {
        private readonly ITagCloudLayouter tagLayouter;
        private readonly WordManager wordManager;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IWordAnalyzer wordAnalyzer;

        public TagGenerator(WordManager wordManager, ITagCloudLayouter tagLayouter,
            IWordAnalyzer wordAnalyzer, IExceptionHandler exceptionHandler)
        {
            this.wordManager = wordManager;
            this.tagLayouter = tagLayouter;
            this.wordAnalyzer = wordAnalyzer;
            this.exceptionHandler = exceptionHandler;
        }

        public Result<List<Tag>> GetTags(IEnumerable<string> words)
        {
            return wordManager.ApplyFilters(words)
                .Then(filtered => wordAnalyzer.WeightWords(filtered))
                .Then(weighted => tagLayouter.GetCloudTags(weighted))
                .Then(tags => tags.ToList())
                .RefineError("Failed, trying to get Cloud Tags")
                .OnFail(exceptionHandler.HandleException);
        }
    }
}