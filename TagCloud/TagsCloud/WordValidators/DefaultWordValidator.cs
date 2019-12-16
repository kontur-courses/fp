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
            var partOfSpechWord = parsePartOfSpeechRegex.Match(myStem.Analysis(word)).Groups[1].Value;
            var isValidWord = boringWords
                .Then(boringWord => boringWord.FirstOrDefault(ignoredWord => ignoredWord.Equals(word, StringComparison.InvariantCultureIgnoreCase)) == null)
                .Then(thisWordIsBoring => word.Length != 0
                && !ignoringPartsOfSpeech.Contains(partOfSpechWord)
                && !int.TryParse(word, out var res) && thisWordIsBoring)
                .OnFail(error => Result.Fail<bool>(error));
            if (isValidWord.IsSuccess)
                viewedWords.Add(word, isValidWord.Value);
            return isValidWord;
        }
    }
}