using System.Collections.Generic;
using System.Linq;
using DeepMorphy;
using System.Text.RegularExpressions;
using TagsCloudVisualization.Structures;
using TagsCloudVisualization.Converter;
using TagsCloudVisualization.Constunts;
using TagsCloudVisualization.Results;
using System;

namespace TagsCloudVisualization.WordAnalyzers
{
    public class DeepMorphyAnalyzer : IMorphAnalyzer, IConverter<string>
    {
        private const string partOfSpeachTag = "post";
        private static readonly Regex wordsFinder = new Regex(@"\W+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Dictionary<string, string> partsOfSpeach = new Dictionary<string, string>
        {
            { "adjf", PartsOfSpeach.Adjective },
            { "adjs", PartsOfSpeach.Adjective },
            { "comp", PartsOfSpeach.Adjective },
            { "advb", PartsOfSpeach.Adverb },
            { "intj", PartsOfSpeach.Interjection },
            { "numb", PartsOfSpeach.Numeral },
            { "noun", PartsOfSpeach.Noun },
            { "npro", PartsOfSpeach.Noun },
            { "verb", PartsOfSpeach.Verb },
            { "infn", PartsOfSpeach.Verb },
            { "prtf", PartsOfSpeach.Participle },
            { "prts", PartsOfSpeach.Participle },
            { "grnd", PartsOfSpeach.Gerund }
        };
        private static readonly MorphAnalyzer morph = new MorphAnalyzer(withLemmatization: true, useEnGrams: true);

        public IEnumerable<Result<WordInfo>> AnalyzeText(string text)
        {
            return wordsFinder
                     .Split(text)
                     .Select(word => Result.Of(() => new WordInfo(GetStandardForm(word), Convert(DefinePartOfSpeech(word))))
                                           .OnFail(error => Console.WriteLine($"Word '{word}' won't be used in tag cloud," +
                                                    $" because this is boring or foreign word")));
        }

        public string DefinePartOfSpeech(string word)
            => morph.Parse(new List<string>() { word }).First().BestTag[partOfSpeachTag];
        

        public string GetStandardForm(string word)
            => morph.Parse(new List<string>() { word }).First().Text;
        

        public string Convert(string obj) => partsOfSpeach[obj];
    }
}
