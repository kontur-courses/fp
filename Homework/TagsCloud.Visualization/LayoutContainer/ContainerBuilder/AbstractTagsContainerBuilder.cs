using System.Collections.Generic;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.LayoutContainer.ContainerBuilder
{
    public abstract class AbstractTagsContainerBuilder
    {
        public abstract TagsContainer Build();
        protected abstract TagsContainerBuilder AddWord(Word word, int minCount, int maxCount);

        public AbstractTagsContainerBuilder AddWords(IEnumerable<Word> wordsToBuild, int minCount, int maxCount)
        {
            foreach (var word in wordsToBuild) AddWord(word, minCount, maxCount);
            return this;
        }
    }
}