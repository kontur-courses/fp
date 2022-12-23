using System.Collections.Generic;
using System.Linq;
using DeepMorphy;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.Infrastructure.Analyzer
{
    public class Analyzer : IAnalyzer
    {
        private readonly Dictionary<PartSpeech, string> converter = new()
        {
            [PartSpeech.Noun] = "сущ",
            [PartSpeech.Adjective] = "прил",
            [PartSpeech.Verb] = "гл",
            [PartSpeech.Infinitive] = "инф_гл",
            [PartSpeech.Participle] = "прич",
            [PartSpeech.Gerund] = "деепр",
            [PartSpeech.Adverb] = "нареч",
            [PartSpeech.Conjunction] = "союз",
            [PartSpeech.Pronoun] = "мест",
            [PartSpeech.Particle] = "част",
            [PartSpeech.Preposition] = "предл",
            [PartSpeech.Interjection] = "межд",
            [PartSpeech.Unknown] = "неизв"
        };

        private readonly AnalyzerSettings settings;

        public Analyzer(AnalyzerSettings settings)
        {
            this.settings = settings;
        }

        public Result<IEnumerable<IWeightedWord>> CreateWordFrequenciesSequence(IEnumerable<string> words)
        {
            var result = new Dictionary<string, int>();

            var remainingWords = Analyze(words);
            if (!remainingWords.IsSuccess)
                return Result.Fail<IEnumerable<IWeightedWord>>(remainingWords.Error);

            foreach (var word in remainingWords.Value)
            {
                if (!result.ContainsKey(word))
                    result[word] = 0;
                result[word]++;
            }

            return Result.Ok(result
                .Select(pair => new WeightedWord { Weight = pair.Value, Word = pair.Key })
                .Cast<IWeightedWord>());
        }

        private Result<IEnumerable<string>> Analyze(IEnumerable<string> words)
        {
            var analyzer = new MorphAnalyzer(settings.Lemmatization, withTrimAndLower: true);

            var excludedTags = settings.ExcludedParts
                .Select(p => converter[p])
                .ToHashSet();

            var selectedTags = settings.SelectedParts
                .Select(p => converter[p])
                .ToHashSet();

            var parsedWords = Result.Of(() => analyzer
                .Parse(words.Where(s => s.Length > 0)));

            return !parsedWords.IsSuccess
                ? Result.Fail<IEnumerable<string>>(
                    "Ошибка внутренней библиотеки, попробуйте убрать пустые строки из файла")
                : Result.Ok(parsedWords.Value
                    .Where(m => m.Tags.All(t => !excludedTags.Contains(t["чр"]))
                                && (selectedTags.Count == 0 ||
                                    selectedTags.Contains(m.BestTag["чр"])))
                    .Where(m => !settings.Lemmatization || m.BestTag.HasLemma)
                    .Select(m => settings.Lemmatization ? m.BestTag.Lemma : m.Text));
        }
    }
}