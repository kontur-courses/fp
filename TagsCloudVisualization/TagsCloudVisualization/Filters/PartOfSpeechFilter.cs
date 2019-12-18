using ResultPatterLibrary;
using System.Collections.Generic;
using TagsCloudVisualization.Constunts;
using TagsCloudVisualization.Structures;
using TagsCloudVisualization.WordAnalyzers;


namespace TagsCloudVisualization.Filters
{
    public class PartOfSpeechFilter: IFilter
    {
        private IMorphAnalyzer Analyzer { get;  set; }

        public PartOfSpeechFilter(IMorphAnalyzer analyzer)
        {
            Analyzer = analyzer;
        }

        public static List<string> WhiteList { get; private set; } = new List<string>
        {
            PartsOfSpeach.Adjective,
            PartsOfSpeach.Gerund,
            PartsOfSpeach.Noun,
            PartsOfSpeach.Verb,
            PartsOfSpeach.Numeral,
            PartsOfSpeach.Participle,
            PartsOfSpeach.Pronoun,
            PartsOfSpeach.Adverb
        };

        public bool Filter(Result<WordInfo> wordInfo) 
            => wordInfo.IsSuccess && wordInfo.Value.StandartForm != string.Empty && WhiteList.Contains(wordInfo.Value.PartOfSpeech);

        public IEnumerable<string> GetFilteredValues(string textToFilter)
        {
            foreach (var word in Analyzer.AnalyzeText(textToFilter))
            {
                var isValid = Filter(word);
                if (isValid)
                    yield return word.Value.StandartForm;
            }
        }
    }
}
