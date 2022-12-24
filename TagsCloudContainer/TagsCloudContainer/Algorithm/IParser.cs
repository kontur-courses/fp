using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Algorithm
{
    public interface IParser
    {
        Result<Dictionary<string, int>> CountWordsInFile(string pathToFile);
        Result<HashSet<string>> FindWordsInFile(string pathToFile);
    }
}
