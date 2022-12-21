using System;
using System.Collections.Generic;
using System.Linq;
using DeepMorphy;
using DeepMorphy.Model;
using TagsCloud.Interfaces;

namespace TagsCloud.Validators
{
    public class MorphsValidator : IMorphsValidator
    {
        public ResultHandler<IEnumerable<MorphInfo>> ParseOnMorphs(IEnumerable<string> words)
        {
            var morphParser = new MorphAnalyzer();
            IEnumerable<MorphInfo> morphInfo;

            try
            {
                morphInfo = morphParser.Parse(words);
            }
            catch
            {
                throw new Exception("DeepMorphy Exception");
            }

            var handler = new ResultHandler<IEnumerable<MorphInfo>>(morphInfo);

            if (morphInfo.Count() != 0)
            {
                return handler;
            }

            return handler.Fail("Empty words enum");
        }
    }
}