
using DeepMorphy.Model;
using DeepMorphy;
using System.Collections.Generic;

namespace TagsCloud.Interfaces
{
    public interface IMorphsValidator
    {
        public Result<IEnumerable<MorphInfo>> ParseOnMorphs(IEnumerable<string> words);
    }
}
