using System.Collections.Generic;
using DeepMorphy.Model;

namespace TagsCloud.Interfaces
{
    public interface INormalFormParser
    {
        public IEnumerable<string> Normalize(IEnumerable<MorphInfo> clearMorphs);
    }
}