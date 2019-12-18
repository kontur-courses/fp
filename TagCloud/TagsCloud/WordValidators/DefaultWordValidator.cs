using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MyStemWrapper;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;

namespace TagsCloud.WordValidators
{
    public class DefaultWordValidator : IWordValidator
    {
        private readonly string[] ignoringPartsOfSpeech;
        private readonly MyStem myStem;
        private readonly Regex parsePartOfSpeechRegex;
        private readonly Dictionary<string, bool> viewedWords;

        public DefaultWordValidator(WordValidatorSettings wordValidatorSettings, MyStem myStem)
        {
            this.myStem = myStem;
            parsePartOfSpeechRegex = new Regex(@"\w*?=(\w+)", RegexOptions.Compiled);
            ignoringPartsOfSpeech = wordValidatorSettings.ignoringPartsOfSpeech;
            viewedWords = new Dictionary<string, bool>();
        }

        public Result<bool> IsValidWord(string word)
        {
            if (viewedWords.TryGetValue(word, out var previousResultValidation))
                return previousResultValidation.AsResult();
            var speechPartOfCurrentWord = parsePartOfSpeechRegex.Match(myStem.Analysis(word)).Groups[1].Value;
            var wordIsValid = word.Length != 0 && !ignoringPartsOfSpeech.Contains(speechPartOfCurrentWord)
                                               && !int.TryParse(word, out _);
            viewedWords.Add(word, wordIsValid);
            return wordIsValid.AsResult();
        }
    }
}