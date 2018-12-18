using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Filtering
{
    public interface IBoringWordsRepository
    {
        IEnumerable<string> Words { get; }
        Result<None> LoadWords(string inputPath);
        
    }
}