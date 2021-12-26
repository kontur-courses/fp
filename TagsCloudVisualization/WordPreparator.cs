using System.Collections.Generic;
using System.Linq;
using DeepMorphy;
using DeepMorphy.Model;
using TagsCloudVisualization.Enums;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class WordPreparator : IWordPreparator
    {
        private readonly HashSet<string> excludedSpeechParts = new();
        private readonly MorphAnalyzer morphAnalyzer;

        private readonly Dictionary<SpeechPart, string> speechPartsDictionary = new()
        {
            { SpeechPart.Noun, "сущ" },
            { SpeechPart.Adjective, "прил" },
            { SpeechPart.Verb, "гл" },
            { SpeechPart.AdverbialParticiple, "деепр" },
            { SpeechPart.Preposition, "предл" }
        };

        public WordPreparator(MorphAnalyzer morphAnalyzer)
        {
            this.morphAnalyzer = morphAnalyzer;
        }

        public Result<IEnumerable<string>> GetPreparedWords(IEnumerable<string> unpreparedWords)
        {
            if (unpreparedWords == null)
                return Result.Fail<IEnumerable<string>>("unpreparedWords was null");

            var words = morphAnalyzer.Parse(unpreparedWords).ToArray();

            return Result.Ok(words
                .Where(info => !IsMorphInfoExcluded(info))
                .Select(t => t.BestTag.Lemma));
        }

        public WordPreparator Exclude(IEnumerable<SpeechPart> speechParts)
        {
            foreach (var speechPart in speechParts)
                AddExcludedSpeechPart(speechPart);

            return this;
        }

        public WordPreparator Exclude(SpeechPart speechPart)
        {
            AddExcludedSpeechPart(speechPart);

            return this;
        }

        private void AddExcludedSpeechPart(SpeechPart speechPart)
        {
            excludedSpeechParts.Add(speechPartsDictionary[speechPart]);
        }

        private bool IsMorphInfoExcluded(MorphInfo morphInfo)
        {
            return excludedSpeechParts.Contains(morphInfo.BestTag.GramsDic["чр"]);
        }
    }
}