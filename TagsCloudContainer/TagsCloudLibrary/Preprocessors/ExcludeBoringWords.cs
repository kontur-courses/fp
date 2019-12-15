using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using TagsCloudLibrary.MyStem;

namespace TagsCloudLibrary.Preprocessors
{
    public class ExcludeBoringWords : IPreprocessor
    {
        private readonly IEnumerable<Word.PartOfSpeech> partOfSpeechWhitelist;

        public int Priority { get; } = 20;

        public ExcludeBoringWords(BoringWordsConfig boringWordsConfig)
        {
            partOfSpeechWhitelist = boringWordsConfig.PartOfSpeechWhitelist;
        }

        public IEnumerable<string> Act(IEnumerable<string> words)
        {
            var myStem = new MyStemProcess();
            var (_, isFailure, value) = myStem.GetWordsWithGrammar(words);

            if (isFailure)
                return new List<string>();

            return value
                .Where(word => (partOfSpeechWhitelist.Contains(word.Grammar.PartOfSpeech)))
                .Select(word => word.InitialString);
        }
    }
}
