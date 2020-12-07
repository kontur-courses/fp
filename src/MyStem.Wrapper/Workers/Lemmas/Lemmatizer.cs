using System;
using FunctionalStuff;
using MyStem.Wrapper.Enums;
using MyStem.Wrapper.Wrapper;

namespace MyStem.Wrapper.Workers.Lemmas
{
    public class Lemmatizer : ILemmatizer
    {
        private readonly Result<IMyStem> myStem;

        public Lemmatizer(IMyStemBuilder myStemBuilder)
        {
            myStem = myStemBuilder.Create(MyStemOutputFormat.Text,
                MyStemOptions.WithoutOriginalForm, MyStemOptions.WithContextualDeHomonymy);
        }

        public Result<string[]> GetWords(string text) =>
            Check.StringIsEmpty(text, "Input")
                .ThenValidate(myStem)
                .Then(x => x.Item2.GetResponse(x.Item1))
                .Then(r => Check.StringIsEmpty(r, "Output"))
                .Then(r => r.Substring(1, r.Length - 2))
                .Then(r => r.Split(new[] {"}{"}, StringSplitOptions.RemoveEmptyEntries));
    }
}