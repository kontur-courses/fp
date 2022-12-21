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
            ResultHandler<IEnumerable<MorphInfo>> handler;

            try
            {
                morphInfo = morphParser.Parse(words);
                handler = new ResultHandler<IEnumerable<MorphInfo>>(morphInfo);
            }
            catch
            {
                handler = new ResultHandler<IEnumerable<MorphInfo>>(null).Fail("DeepMorphy Exception");
                return handler;
            }

            if (morphInfo.Count() != 0)
            {
                return handler;
            }

            return handler.Fail("Empty words enum");
        }
    }
}