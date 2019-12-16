using TagsCloud.Interfaces;
using System.Linq;
using System.Collections.Generic;
using MyStemWrapper;
using System.Text.RegularExpressions;
using System;
using TagsCloud.Spliters;
using TagsCloud.ErrorHandling;

namespace TagsCloud.WordValidators
{
    public class DefaultWordValidator : IWordValidator
    {
        private readonly Result<HashSet<string>> boringWords;
        private readonly MyStem myStem;
        private readonly Regex parsePartOfSpeechRegex;
        private readonly string[] ignoringPartsOfSpeech;
        private readonly Dictionary<string, bool> viewedWords;

        public DefaultWordValidator(SpliterByLine textSpliter, ITextReader fileReader, WordValidatorSettings wordValidatorSettings, MyStem myStem)
        {
            this.myStem = myStem;
            boringWords = wordValidatorSettings.pathToBoringWords == "" ? new HashSet<string>().AsResult() :
                fileReader.ReadFile(wordValidatorSettings.pathToBoringWords)
                .Then(text => textSpliter.SplitText(text))
                .Then(words => words.ToHashSet())
                .RefineError("File with boring words could not be read");
            parsePartOfSpeechRegex = new Regex(@"\w*?=(\w+)");
            ignoringPartsOfSpeech = wordValidatorSettings.ignoringPartsOfSpeech;
            viewedWords = new Dictionary<string, bool>();
        }

        public Result<bool> IsValidWord(string word)
        {
            if (viewedWords.TryGetValue(word, out var previousResultValidation))
                return previousResultValidation.AsResult();
            return boringWords
                .Then(boringWord => boringWord.FirstOrDefault(ignoredWord => ignoredWord.Equals(word, StringComparison.InvariantCultureIgnoreCase)) == null)
                .Then(wordIsBoring => word.Length != 0
                && !ignoringPartsOfSpeech.Contains(parsePartOfSpeechRegex.Match(myStem.Analysis(word)).Groups[1].Value)
                && !int.TryParse(word, out var res) && wordIsBoring)
                .Then(wordIsBoring => 
                {
                    viewedWords.Add(word, wordIsBoring);
                    return wordIsBoring;
                })
                .OnFail(error => Result.Fail<bool>(error));
        }
    }
}