using System;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.MyStem
{
    public class Word
    {
        public enum PartOfSpeech
        {
            Adjective,
            Adverb,
            AdverbPronoun,
            AdjectiveNumeral,
            AdjectivePronoun,
            Composite,
            Conjunction,
            Interjection,
            Numeral,
            Particle,
            Preposition,
            Noun,
            NounPronoun,
            Verb
        }

        public class WordGrammar
        {
            public PartOfSpeech PartOfSpeech;
            public string InitialForm;
        }

        private static readonly Regex WordAndLemmaRegex = new Regex(@"^([А-Яа-я\w-]+)\{([А-Яа-я\w=,\|\-\d?]+)\}$");

        public string InitialString { get; }
        public WordGrammar Grammar { get; }

        public static Result<Word> FromMyStemConclusion(string myStemConclusion)
        {
            const string errorMessage = "Given string is not a mystem conclusion";
            string initialString = "", initialForm = "";

            return 
                Result.Ok()
                .Map(() => WordAndLemmaRegex.Match(myStemConclusion))
                .Ensure(match => match.Success, errorMessage)
                .Tap(match => initialString = match.Groups[1].Value)
                .Map(match => match.Groups[2].Value)
                .Map(lemma => lemma.Split('|'))
                .Ensure(possibilities => possibilities[0].Contains("="), errorMessage)
                .Map(possibilities => possibilities[0].Split('='))
                .Tap(possibility => initialForm = possibility[0])
                .Map(possibility => possibility[1])
                .Map(grammarInfo => grammarInfo.Split(',')[0])
                .Bind(PartOfSpeechFromMystem)
                .Map(pos => new Word(initialString, new WordGrammar
                {
                    InitialForm = initialForm,
                    PartOfSpeech = pos
                }));
        }

        public Word(string initialString, WordGrammar grammar)
        {
            InitialString = initialString;
            Grammar = grammar;
        }

        private static Result<PartOfSpeech> PartOfSpeechFromMystem(string partOfSpeechInfo)
        {
            PartOfSpeech pos;
            switch (partOfSpeechInfo)
            {
                case "A":
                    pos = PartOfSpeech.Adjective;
                    break;
                case "ADV":
                    pos = PartOfSpeech.Adverb;
                    break;
                case "ADVPRO":
                    pos = PartOfSpeech.AdverbPronoun;
                    break;
                case "ANUM":
                    pos = PartOfSpeech.AdjectiveNumeral;
                    break;
                case "APRO":
                    pos = PartOfSpeech.AdjectivePronoun;
                    break;
                case "COM":
                    pos = PartOfSpeech.Composite;
                    break;
                case "CONJ":
                    pos = PartOfSpeech.Conjunction;
                    break;
                case "INTJ":
                    pos = PartOfSpeech.Interjection;
                    break;
                case "NUM":
                    pos = PartOfSpeech.Numeral;
                    break;
                case "PART":
                    pos = PartOfSpeech.Particle;
                    break;
                case "PR":
                    pos = PartOfSpeech.Preposition;
                    break;
                case "S":
                    pos = PartOfSpeech.Noun;
                    break;
                case "SPRO":
                    pos = PartOfSpeech.NounPronoun;
                    break;
                case "V":
                    pos = PartOfSpeech.Verb;
                    break;
                default:
                    return Result.Failure<PartOfSpeech>("Wrong part of speech was given. This is probably because of wrong version of mystem you are using.");
            }

            return Result.Ok(pos);
        }
    }
}