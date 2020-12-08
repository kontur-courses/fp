using System.Collections.Generic;
using System.Linq;
using FunctionalStuff.Fails;
using FunctionalStuff.Results;
using MyStem.Wrapper.Enums;
using MyStem.Wrapper.Workers.Grammar.Raw;
using MyStem.Wrapper.Wrapper;
using Newtonsoft.Json;

namespace MyStem.Wrapper.Workers.Grammar
{
    public class GrammarAnalyser : IGrammarAnalyser
    {
        private readonly Result<IMyStem> myStem;

        public GrammarAnalyser(IMyStemBuilder myStemBuilder)
        {
            myStem = myStemBuilder.Create(MyStemOutputFormat.Json,
                MyStemOptions.WithoutOriginalForm, MyStemOptions.WithContextualDeHomonymy,
                MyStemOptions.WithGrammarInfo
            );
        }

        public Result<AnalysisResultRaw[]> GetRawResult(string text) =>
            Fail.If(text, $"{nameof(GrammarAnalyser)} input").NullOrEmpty()
                .ThenJoin(myStem, (t, ms) => new {Text = t, MyStem = ms})
                .Then(x => x.MyStem.GetResponse(x.Text))
                .Then(x => x.FailIf($"{nameof(IMyStem)} output").NullOrEmpty())
                .Then(r => JsonConvert.DeserializeObject<IList<AnalysisResultRaw>>(r).ToArray())
                .Then(x => x.FailIf("Deserialized value").NullOrEmpty());
    }
}