using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloud.WordPrework
{
    public interface IWordAnalyzer
    {
        Result<Dictionary<string, int>> GetWordFrequency(HashSet<PartOfSpeech> boringPartsOfSpeech);

        Result<Dictionary<string, int>> GetSpecificWordFrequency(IEnumerable<PartOfSpeech> partsOfSpeech);
    }
}
