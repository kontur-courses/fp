using DeepMorphy;

namespace TagsCloudContainer.Extensions
{
    public static class MorphAnalyzerExtensions
    {
        public static IReadOnlyDictionary<string, string> GetGrams(this MorphAnalyzer morph, string word)
        {
            return morph.Parse(new[] {word}).First().BestTag.GramsDic;
        }
    }
}
