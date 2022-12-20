using System.Collections.Generic;
using DeepMorphy.Model;

namespace TagsCloud.Interfaces
{
    public interface IMorphsValidator
    {
        public ResultHandler<IEnumerable<MorphInfo>> ParseOnMorphs(IEnumerable<string> words);
    }
}