using System;
using System.Collections.Generic;
using NHunspell;
using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Word_Counting
{
    public class WordNormalizer : IWordNormalizer
    {
        private readonly Hunspell hunspell;

        public WordNormalizer(Hunspell hunspell)
        {
            this.hunspell = hunspell;
        }

        public Result<string> Normalize(string word)
        {
            try
            {
                var lowered = word.ToLower();
                var stemmed = hunspell.Stem(lowered);
                return stemmed.Count != 0
                    ? hunspell.Stem(word.ToLower())[0]
                    : lowered;
            }
            catch (Exception e)
            {
                return Result.Fail<string>(e.Message);
            }
        }
    }
}