using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyStemWrapper;
using Newtonsoft.Json.Linq;
using TagsCloud.ErrorHandler;
using TagsCloud.Properties;

namespace TagsCloud.WordPreprocessing
{
    public class WordsCleaner : IWordsProcessor
    {
        private readonly bool _infinitive;
        private readonly MyStem _stemmer = new MyStem();

        private readonly HashSet<PartOfSpeech> boringPartsOfSpeech = new HashSet<PartOfSpeech>
        {
            PartOfSpeech.Particle,
            PartOfSpeech.Conjunction,
            PartOfSpeech.Pretext,
            PartOfSpeech.PronounNoun,
            PartOfSpeech.PronounAdjective,
            PartOfSpeech.Unknown
        };

        private readonly Dictionary<string, PartOfSpeech> partOfSpeechDenotation =
            new Dictionary<string, PartOfSpeech>
            {
                {"A", PartOfSpeech.Adjective},
                {"ADV", PartOfSpeech.Adverb},
                {"ADVPRO", PartOfSpeech.PronominalAdverb},
                {"ANUM", PartOfSpeech.NumeralAdjective},
                {"APRO", PartOfSpeech.PronounAdjective},
                {"COM", PartOfSpeech.CompositePart},
                {"CONJ", PartOfSpeech.Conjunction},
                {"INTJ", PartOfSpeech.Interjection},
                {"NUM", PartOfSpeech.Numeral},
                {"PART", PartOfSpeech.Particle},
                {"PR", PartOfSpeech.Pretext},
                {"S", PartOfSpeech.Noun},
                {"SPRO", PartOfSpeech.PronounNoun},
                {"V", PartOfSpeech.Verb}
            };

        public WordsCleaner(bool infinitive)
        {
            var stemmerPath = Path.GetTempFileName();
            File.WriteAllBytes(stemmerPath, Resources.mystem);
            _stemmer.PathToMyStem = stemmerPath;
            _stemmer.Parameters = "-i --format json";
            _infinitive = infinitive;
        }

        public Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words)
        {
            if (!File.Exists(_stemmer.PathToMyStem))
            {
                return Result.Fail<IEnumerable<string>>("Can't find path to 'mystem.exe'");
            }
            var processedWords = new List<string>();
            var lastError = "";
            words = words.Select(w => w.ToLower());
            foreach (var word in words)
            {
                var wordData = _stemmer.Analysis(word);
                Result.Ok(wordData)
                    .Then(wd => GetPartOfSpeech(wd, word))
                    .Then(ps =>
                    {
                        var infinitive = GetInfinitiveForm(wordData, word);
                        if (!boringPartsOfSpeech.Contains(ps))
                            processedWords.Add(_infinitive && infinitive.IsSuccess ? infinitive.Value : word);
                    }).OnFail(e => lastError = e);
            }

            return lastError != "" ? Result.Fail<IEnumerable<string>>(lastError) : processedWords;
        }

        private Result<PartOfSpeech> GetPartOfSpeech(string jsonAnalysis, string word)
        {
            var jsonArray = JArray.Parse(jsonAnalysis);
            if (!jsonAnalysis.Contains("gr")) return PartOfSpeech.Unknown;
            return Result.Of(() =>
                {
                    var designation = jsonArray[0]["analysis"][0]["gr"].ToString().Split(',', '=')[0];
                    return partOfSpeechDenotation[designation];
                })
                .ReplaceError(e => $"Can't define the type of the word: '{word}'");
            ;
        }

        private Result<string> GetInfinitiveForm(string jsonAnalysis, string word)
        {
            var jsonArray = JArray.Parse(jsonAnalysis);
            return Result.Of(() => jsonArray[0]["analysis"][0]["lex"].ToString())
                .ReplaceError(e => $"Can't define the infinitive form of the word '{word}'");
        }
    }
}