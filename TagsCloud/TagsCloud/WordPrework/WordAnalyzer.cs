using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MyStemWrapper;
using Newtonsoft.Json.Linq;
using TagsCloud.ErrorHandling;


namespace TagsCloud.WordPrework
{
    public class WordAnalyzer: IWordAnalyzer
    {
        private readonly Dictionary<string, PartOfSpeech> partsOfSpeechDesignations =
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

        private readonly HashSet<PartOfSpeech> standartBoringPartsOfSpeech = new HashSet<PartOfSpeech>
        {
            PartOfSpeech.Particle, PartOfSpeech.Conjunction,  PartOfSpeech.Pretext,  PartOfSpeech.PronominalAdverb,
            PartOfSpeech.PronounNoun, PartOfSpeech.PronounAdjective
        };

        private readonly Dictionary<string, int> WordsFrequency = new Dictionary<string, int>();
        private MyStem stemmer = new MyStem();
        private char[] delimiters = new char[] {',','.',' ',':',';','(',')', '—', '–'};

        public WordAnalyzer(IEnumerable<Result<string>> words, bool useInfinitiveForm = false)
        {
            stemmer.PathToMyStem = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mystem.exe");
            stemmer.Parameters = "-i --format json";
            foreach (var word in words)
            {
                if (!word.IsSuccess)
                    break;
                String wordForm;
                if (useInfinitiveForm)
                {
                    var infinitiveForm = GetInfinitiveForm(word.Value);
                    wordForm = infinitiveForm.IsSuccess ? infinitiveForm.Value : word.Value.ToLower();
                }
                else
                    wordForm =  word.Value.ToLower();

                if (WordsFrequency.ContainsKey(wordForm))
                    WordsFrequency[wordForm] += 1;
                else
                    WordsFrequency[wordForm] = 1;
            }
        }

        public Result<Dictionary<string, int>> GetWordFrequency(HashSet<PartOfSpeech> newBoringPartOfSpeech = null)
        {
            if (newBoringPartOfSpeech == null || newBoringPartOfSpeech.Count == 0)
                newBoringPartOfSpeech = standartBoringPartsOfSpeech;
            var result = new Dictionary<string, int>();
            foreach (var item in WordsFrequency)
            {
                var partOfSpeech = GetPartOfSpeech(item.Key);
                if (!partOfSpeech.IsSuccess)
                    return new Result<Dictionary<string, int>>(partOfSpeech.Error)
                        .RefineError("Error in getting word frequency");

                if (!newBoringPartOfSpeech.Contains(partOfSpeech.Value))
                    result[item.Key] = item.Value;
            }
            return result;
        }

        public Result<Dictionary<string, int>> GetSpecificWordFrequency(IEnumerable<PartOfSpeech> partsOfSpeech)
        {
            var result = new Dictionary<string, int>();
            var neededParts = new HashSet<PartOfSpeech>(partsOfSpeech);
            foreach (var item in WordsFrequency)
            {
                var partOfSpeech = GetPartOfSpeech(item.Key);
                if (!partOfSpeech.IsSuccess)
                    return new Result<Dictionary<string, int>>(partOfSpeech.Error)
                        .RefineError("Error in getting specific word frequency");

                if (neededParts.Contains(partOfSpeech.Value))
                    result[item.Key] = item.Value;
            }
            return result;
        }

        private Result<string> GetInfinitiveForm(string word)
        {
            var jsonAnalysis = stemmer.Analysis(word);
            var jsonArray = JArray.Parse(jsonAnalysis);
            var infinitiveForm = Result.Of(() => jsonArray[0]["analysis"][0]["lex"].ToString())
                .ReplaceError(e => $"Exception in getting the infinitive form of the word '{word}'");
            return infinitiveForm;
        }

        private Result<PartOfSpeech> GetPartOfSpeech(string word)
        {
            var jsonAnalysis = stemmer.Analysis(word);
            var jsonArray = JArray.Parse(jsonAnalysis);
            var partOfSpeech = Result.Of(() =>
                {
                    var designation = jsonArray[0]["analysis"][0]["gr"].ToString().Split(',', '=')[0];
                    return partsOfSpeechDesignations[designation];
                })
                .ReplaceError(e => $"Exception in getting the part of the speech of the word '{word}'");
            return partOfSpeech;
        }
    }
}
