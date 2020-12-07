using System.Collections.Generic;
using System.Linq;
using FunctionalStuff;
using MyStem.Wrapper.Workers.Grammar;
using MyStem.Wrapper.Workers.Grammar.Parsing;
using MyStem.Wrapper.Workers.Grammar.Parsing.Models;

namespace TagCloud.Core.Text.Preprocessing
{
    public class MyStemSpeechPartWordsFilter : ISpeechPartWordsFilter
    {
        private readonly IUserNotifier notifier;
        private readonly IGrammarAnalyser analyser;
        private readonly IGrammarAnalysisParser parser;

        public MyStemSpeechPartWordsFilter(IGrammarAnalyser analyser,
            IGrammarAnalysisParser parser,
            IUserNotifier notifier)
        {
            this.analyser = analyser;
            this.parser = parser;
            this.notifier = notifier;
        }

        public IEnumerable<string> OnlyWithSpeechPart(
            IEnumerable<string> words,
            ISet<MyStemSpeechPart> includedSpeechParts)
        {
            return GetValidOrOriginalWords(includedSpeechParts, words.ToArray());
        }

        private IEnumerable<string> GetValidOrOriginalWords(
            ISet<MyStemSpeechPart> includedSpeechParts,
            string[] wordsArray)
        {
            return analyser.GetRawResult(string.Join(" ", wordsArray))
                .Then(x => x.Select(r => r.ParseWith(parser))
                    .Where(r => r.Entries.Any(e => includedSpeechParts.Contains(e.SpeechPart)))
                    .Select(r => r.Text))
                .GetValueOrHandleError(wordsArray, notifier.Notify);
        }
    }
}