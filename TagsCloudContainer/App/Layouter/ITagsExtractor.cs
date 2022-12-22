using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.App.Layouter
{
    public interface ITagsExtractor
    {
        public Result<Dictionary<string, int>> FindAllTagsInText(string text);
    }
}
