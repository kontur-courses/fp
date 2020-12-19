using System.Linq;
using NHunspell;

namespace TagsCloud.Core
{
    public static class StringExtentions
    {
        public static string Normalize(this string word, Hunspell hunspell)
        {
            var stems = hunspell.Stem(word);
            return stems.Count > 0 ? stems.FirstOrDefault() : word;
        }
    }
}