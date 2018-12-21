using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ResultOf;

namespace TagCloud
{
    public class TextAnalyzer : ITextAnalyzer
    {
        private readonly IWordSource stopWords;
        private readonly IWordSource partsOfSpeech;
        private readonly bool useStems;
        private IWordSource Text { get; }
        private IEnumerable<Word> WordList { get; set; }
        private int MaxFrequency { get; set; }
        
        public TextAnalyzer(IWordSource text, IWordSource stopWords,
            IWordSource partsOfSpeech = null, bool useStems = false)
        {
            this.stopWords = stopWords;
            this.partsOfSpeech = partsOfSpeech;
            this.useStems = useStems;
            Text = text;
        }

        public Result<IEnumerable<Word>> GetWordList()
        {
            if (WordList != null)
                return Result.Ok(WordList);
            var statsResult = GetWordStats();
            return !statsResult.IsSuccess 
                ? Result.Fail<IEnumerable<Word>>(statsResult.Error).RefineError("Could not get word list") 
                : Result.Ok(WordList);
        }

        public Result<int> GetMaxFrequency()
        {
            if (MaxFrequency != 0)
                return Result.Ok(MaxFrequency);
            var statsResult = GetWordStats();
            return !statsResult.IsSuccess 
                ? Result.Fail<int>(statsResult.Error).RefineError("Could not get maximum word frequency") 
                : Result.Ok(MaxFrequency);
        }

        private Result<None> GetWordStats()
        {
            var wordFrequencies = new Dictionary<string, int>();
            var maxFrequency = 0;
            var words = GetWordsFromSource(Text);
            if (!words.IsSuccess)
                return Result.Fail<None>(words.Error)
                    .RefineError("Could not read words from source");
            
            foreach (var word in words.GetValueOrThrow())
            {
                if (!wordFrequencies.ContainsKey(word))
                    wordFrequencies[word] = 0;
                if (wordFrequencies[word]++ > maxFrequency)
                    maxFrequency++;
            }

            MaxFrequency = maxFrequency;
            WordList = wordFrequencies.Select(kvp => new Word(kvp.Key, kvp.Value)).ToList();

            return Result.Ok();
        }

        private Result<IEnumerable<string>> GetWordsFromSource(IWordSource source)
        {
            return Text.GetWords()
                .Then(words => words
                .Select(w => w.ToLowerInvariant())
                .Select(w => Regex.Replace(w, @"(\W|\d)", string.Empty, RegexOptions.Compiled))
                .Where(w => !stopWords.GetWords().GetValueOrThrow().Contains(w)));
        }
    }
}
