using System.Collections.Generic;
using FunctionalStuff.Results;
using MyStem.Wrapper.Workers.Grammar.Parsing.Models;

namespace TagCloud.Core.Text.Preprocessing
{
    public interface ISpeechPartWordsFilter
    {
        Result<IEnumerable<string>> OnlyWithSpeechPart(
            IEnumerable<string> words,
            ISet<MyStemSpeechPart> speechParts);
    }
}