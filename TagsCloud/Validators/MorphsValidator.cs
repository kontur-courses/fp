using DeepMorphy;
using DeepMorphy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagsCloud.Interfaces;
using TagsCloud.TextWorkers;

namespace TagsCloud.Validators
{
    public class MorphsValidator : IMorphsValidator
    {
        public Result<IEnumerable<MorphInfo>> ParseOnMorphs(IEnumerable<string> words)
        {
            var morphParser = new MorphAnalyzer();
            var morphInfo = morphParser.Parse(words);

            if (morphInfo.Count() == 0)
            {
                return Result<IEnumerable<MorphInfo>>.Fail<IEnumerable<MorphInfo>>("Empty words enum");
            }
            else
            {
                return Result<IEnumerable<MorphInfo>>.Ok(morphInfo);
            }
        }
    }
}
