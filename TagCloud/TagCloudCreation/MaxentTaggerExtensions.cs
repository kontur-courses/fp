using System;
using System.Linq;
using edu.stanford.nlp.tagger.maxent;
using Result;
using static Result.Result;

namespace TagCloudCreation
{
    public static class MaxentTaggerExtensions
    {
        public static Result<string> GetTag(this MaxentTagger tagger, string word)
        {
            try
            {
                return tagger.tagString(word)
                             .Split('_')
                             .Last()
                             .TrimEnd();
            }
            catch (InvalidOperationException ex)
            {
                return Fail<string>(ex.Message);
            }
        }
    }
}
