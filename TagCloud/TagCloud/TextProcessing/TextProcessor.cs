using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagCloud.TextProcessing
{
    internal class TextProcessor : ITextProcessor
    {
        private readonly IMorphologyAnalyzer _morphologyAnalyzer;
        private readonly IFileProvider _textProvider;

        public TextProcessor(IFileProvider textProvider, IMorphologyAnalyzer morphologyAnalyzer)
        {
            _textProvider = textProvider;
            _morphologyAnalyzer = morphologyAnalyzer;
        }

        public Result<Dictionary<string, int>> GetWordsWithFrequency(ITextProcessingOptions options, string filePath)
        {
            return _textProvider.GetTxtFilePath(filePath)
                .Then(_morphologyAnalyzer.GetLexemesFrom)
                .Then(lexemes =>
                {
                    var excluded = lexemes
                        .Where(r => !options.ExcludePartOfSpeech.Contains(r.PartOfSpeech)
                                    || options.IncludeWords.Contains(r.Lemma))
                        .Select(r => r.Lemma)
                        .Where(w => !options.ExcludeWords.Contains(w));
                    return GetMostCommonWords(excluded, options.Amount);
                });
        }

        private static Dictionary<string, int> GetMostCommonWords(IEnumerable<string> words, int amount)
        {
            return words
                .GroupBy(s => s)
                .OrderByDescending(g => g.Count())
                .Take(amount)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}