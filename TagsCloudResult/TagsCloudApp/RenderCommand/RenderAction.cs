using System.Collections.Generic;
using System.Linq;
using TagsCloudApp.Actions;
using TagsCloudApp.WordsLoading;
using TagsCloudContainer;

namespace TagsCloudApp.RenderCommand
{
    public class RenderAction : IAction
    {
        private readonly ITagsCloudDirector tagsCloudDirector;
        private readonly IBitmapSaver bitmapSaver;
        private readonly IWordsProvider wordsProvider;
        private readonly IEnumerable<IAction> actions;

        public RenderAction(
            ITagsCloudDirector tagsCloudDirector,
            IBitmapSaver bitmapSaver,
            IWordsProvider wordsProvider,
            IEnumerable<IAction> actions)
        {
            this.tagsCloudDirector = tagsCloudDirector;
            this.bitmapSaver = bitmapSaver;
            this.wordsProvider = wordsProvider;
            this.actions = actions;
        }

        public Result<None> Perform()
        {
            return actions.Select(a => a.Perform())
                .CombineResults()
                .Then(_ => wordsProvider.GetWords())
                .Then(tagsCloudDirector.RenderWords)
                .Then(bmp =>
                {
                    bitmapSaver.Save(bmp);
                    bmp.Dispose();
                });
        }
    }
}