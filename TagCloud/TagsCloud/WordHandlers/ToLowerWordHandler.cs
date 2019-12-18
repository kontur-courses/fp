using TagsCloud.Interfaces;

namespace TagsCloud.WordHandlers
{
    public class ToLowerWordHandler : IWordHandler
    {
        public string ProcessWord(string word)
        {
            return word.ToLowerInvariant();
        }
    }
}