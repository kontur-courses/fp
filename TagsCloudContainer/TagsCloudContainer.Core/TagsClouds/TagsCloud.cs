using TagsCloudContainer.Core.Results;
using TagsCloudContainer.Core.Drawer.Interfaces;
using TagsCloudContainer.Core.TagsClouds.Interfaces;
using TagsCloudContainer.Core.WordsParser.Interfaces;

namespace TagsCloudContainer.Core.TagsClouds
{
    public class TagsCloud : ITagsCloud
    {
        private readonly IWordsAnalyzer _wordsAnalyzer;
        private readonly IRectangleLayout _rectangleLayout;

        public TagsCloud(IWordsAnalyzer wordsAnalyzer, IRectangleLayout rectangleLayout)
        {
            _wordsAnalyzer = wordsAnalyzer;
            _rectangleLayout = rectangleLayout;
        }

        public Result<None> CreateTagCloud() =>
            _wordsAnalyzer.AnalyzeWords()
                .Then(_rectangleLayout.PlaceWords)
                .Then(_ => _rectangleLayout.DrawLayout());

        public Result<None> SaveTagCloud() 
            => Result.OfAction(() => _rectangleLayout.SaveLayout());
    }
}