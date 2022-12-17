using System.Collections.Generic;
using DeepMorphy.Model;

namespace TagsCloud.Interfaces
{
    public interface IMorphsFilter
    {
        public IEnumerable<MorphInfo> FilterRedundantWords(IEnumerable<MorphInfo> morphs);
    }
}