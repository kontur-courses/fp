using System.Collections.Generic;
using DeepMorphy.Model;

namespace TagsCloud.Interfaces
{
    public interface IMorphsParser
    {
        public IEnumerable<MorphInfo> GetMorphs(string filePath);
    }
}