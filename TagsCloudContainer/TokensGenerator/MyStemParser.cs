using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.TokensGenerator
{
    public class MyStemParser : ITokensParser
    {
        private readonly IMysteam mysteam;

        public MyStemParser(IMysteam mysteam)
        {
            this.mysteam = mysteam;
        }

        public Result<IEnumerable<string>> GetTokens(string str)
        {
            if (str == null)
                throw new ArgumentNullException();
            var res = Result.Ok(str)
                .Then(s => s.Replace("\r\n", " "))
                .Then(mysteam.GetWords)
                .Then(words =>
                    words.Select(el => el.SourceWord.Analysis.FirstOrDefault()?.Lex ?? el.SourceWord.Text.ToLower()));
            return res;
        }
    }
}