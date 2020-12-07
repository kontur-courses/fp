using System.Collections.Generic;
using System.Linq;
using FunctionalStuff;
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
            Check.StringIsEmpty(text, "Input")
                .ThenValidate(myStem)
                .Then(x => x.Item2.GetResponse(x.Item1))
                .Then(r => Check.StringIsEmpty(r, "Output"))
                .Then(r => JsonConvert.DeserializeObject<IList<AnalysisResultRaw>>(r).ToArray());
    }
}