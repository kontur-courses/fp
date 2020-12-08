using System.Collections.Generic;
using System.Linq;
using FunctionalStuff.Results;
using FunctionalStuff.Results.Fails;
using MyStem.Wrapper.Workers.Grammar;
using MyStem.Wrapper.Workers.Grammar.Parsing;
using MyStem.Wrapper.Workers.Grammar.Parsing.Models;

namespace TagCloud.Core.Text.Preprocessing
{
    public class MyStemSpeechPartWordsFilter : ISpeechPartWordsFilter
    {
        private readonly IGrammarAnalyser analyser;
        private readonly IGrammarAnalysisParser parser;

        public MyStemSpeechPartWordsFilter(IGrammarAnalyser analyser,
            IGrammarAnalysisParser parser)
        {
            this.analyser = analyser;
            this.parser = parser;
        }

        public Result<IEnumerable<string>> OnlyWithSpeechPart(
            IEnumerable<string> words,
            ISet<MyStemSpeechPart> includedSpeechParts)
        {
            return analyser.GetRawResult(string.Join(" ", words))
                .Then(x => Fail.If(x, $"{nameof(IGrammarAnalyser)} output").NullOrEmpty())
                .Then(x => x.Select(r => r.ParseWith(parser))
                    .Where(r => r.Entries.Any(e => includedSpeechParts.Contains(e.SpeechPart)))
                    .Select(r => r.Text))
                .Then(x => Fail.If(x, "Words with specified speech part collection").NullOrEmpty())
                .Then(x => x.AsEnumerable())
                .RefineError("Cannot filter words by speech parts");
        }
    }
}