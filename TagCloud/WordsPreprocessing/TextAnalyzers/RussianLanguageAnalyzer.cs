using System.Collections.Generic;
using System.Linq;
using TagCloud.WordsPreprocessing.WordsSelector;
using YandexMystem.Wrapper;
using YandexMystem.Wrapper.Enums;
using YandexMystem.Wrapper.Models;

namespace TagCloud.WordsPreprocessing.TextAnalyzers
{
    /// <summary>
    /// Приводит слова русского языка к начальной форме 
    /// </summary>
    [Name("RussianLanguageAnalyzer")]
    public class RussianLanguageAnalyzer : ITextAnalyzer
    {
        private readonly WordSelectorSettings wordSelectorSettings;

        private readonly Mysteam mystem;

        public RussianLanguageAnalyzer(WordSelectorSettings wordSelectorSettings)
        {
            this.wordSelectorSettings = wordSelectorSettings;
            try
            {
                mystem = new Mysteam();
            }
            catch
            {
                mystem = null;
            }
        }

        public Result<Word[]> GetWords(IEnumerable<string> words, int count)
        {
            var counter = 0;

            if (mystem is null)
                return Result.Fail<Word[]>("Can not include external library. Please select another analyzer");

            return Result.Of(() => words
                .Select(w => mystem.GetWords(w))
                .Where(w =>
                {
                    if (w.Count == 0) return false;
                    counter++;
                    return true;

                })
                .Select(w => ParseWord(w[0]))
                .GroupBy(w => w.Value)
                .Select(w =>
                {
                    var result = w.First();
                    result.Count = w.Count();
                    return result;
                })
                .Where(wordSelectorSettings.CanUseThisWord)
                .Take(count)
                .Select(w =>
                {
                    w.Frequency = (double) w.Count / counter;
                    return w;
                })
                .ToArray(), "Can not parse words. Please select another analyzer");
        }

        private static Word ParseWord(WordModel model)
        {
            SpeechPart speechPart;
            switch (model.Lexems[0].GramPart)
            {
                case GramPartsEnum.Verb:
                    speechPart = SpeechPart.Verb;
                    break;
                case GramPartsEnum.Adjective:
                    speechPart = SpeechPart.Adjective;
                    break;
                case GramPartsEnum.Noun:
                    speechPart = SpeechPart.Noun;
                    break;
                default:
                    speechPart = SpeechPart.Unknown;
                    break;
            }
            return new Word(model.Lexems[0].Lexeme, speechPart);
        }
    }
}
