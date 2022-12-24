using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Algorithm
{
    public interface IWordsCounter
    {
        public Result<Dictionary<string, int>> CountWords(string pathToSource, string pathToCustomBoringWords);
    }
}
