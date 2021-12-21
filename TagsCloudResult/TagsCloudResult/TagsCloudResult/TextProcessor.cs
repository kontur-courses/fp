using DeepMorphy;
using ResultOf;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public static class TextProcessor
    {
        public static Result<IEnumerable<string>> FilterWords(this IEnumerable<string> words, HashSet<string> boringWords)
        {
            return Result.Ok(words).Then(words => words.Select(word => word.ToLower())
                    .Where(word => !boringWords.Contains(word)));
        }

        public static Result<IEnumerable<string>> LemmatizationWords(this IEnumerable<string> words)
        {
            var morph = new MorphAnalyzer(withLemmatization: true, withTrimAndLower: true);
            return Result.Ok(words)
                .Then(words => morph.Parse(words).Select(morphInf => morphInf.Tags[0].Lemma))
                .RefineError("Библиотека DeepMorphy дала сбой");
        }
    }
}
