using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResultOf;

namespace TagCloud.Factories
{
    public class BoringWordsFactory : IBoringWordsFactory
    {
        public Result<HashSet<string>> GetFromFile(string fileName)
        {
            return Result.Of(() => File.ReadAllLines(fileName)
                    .Select(s => s.ToLower().Trim())
                    .ToHashSet());
        }
    }
}
