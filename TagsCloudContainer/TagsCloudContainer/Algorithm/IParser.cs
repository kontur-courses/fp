using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer.Algorithm
{
    public interface IParser
    {
        Dictionary<string, int> CountWordsInFile(string pathToFile);
        HashSet<string> FindWordsInFile(string pathToFile);
    }
}
