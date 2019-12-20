using System;
using System.Collections.Generic;
using System.Linq;
using YandexMystem.Wrapper;

namespace TagsCloudContainer.TokensGenerator
{
    public class MyStemParser : ITokensParser
    {
        private readonly IMysteam mysteam;

        public MyStemParser(IMysteam mysteam)
        {
            this.mysteam = mysteam;
        }

        public IEnumerable<string> GetTokens(string str)
        {
            if (str == null)
                throw new ArgumentNullException();
            var replace = str.Replace("\r\n", " ");
            return mysteam.GetWords(replace).Select(el => el.SourceWord.Analysis.FirstOrDefault()?.Lex ?? el.SourceWord.Text.ToLower());
        }
    }
}