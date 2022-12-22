using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudContainer.App.Layouter;
using DeepMorphy;
using ResultOf;

namespace TagsCloudContainer.App.Layouter
{
    public class TagsExtractor : ITagsExtractor
    {
        private readonly MorphAnalyzer morphAnalyzer;
        private readonly string[] partsOfSpeech;

        public TagsExtractor()
        {
            morphAnalyzer = new MorphAnalyzer(withLemmatization: true);
            partsOfSpeech = new[] { "сущ", "прил", "кр_прил", "гл", "инф_гл", "прич", "кр_прич", "деепр", "нареч" };
        }

        public Result<Dictionary<string, int>> FindAllTagsInText(string text)
        {
            return Result.Of(() => (ChooseNotBoringWordsWithSimpleForm(text.ToLower().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)))
                .GetValueOrThrow()
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count()));
        }

        private Result<IEnumerable<string>> ChooseNotBoringWordsWithSimpleForm(string[] text)
        {
            var result = Result.Of(() => morphAnalyzer.Parse(text)
                .Where(tag => partsOfSpeech.Contains(tag.BestTag.Grams.FirstOrDefault()))
                .Select(tag => tag.BestTag.Lemma)
                .Where(tag => tag != null));
            return (result.Value is null || !result.Value.Any())
                ? Result.Fail<IEnumerable<string>>("В этом тексте нет ни одного тега")
                : result;
        }    
    }
}
