using System.Collections.Generic;
using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.Interfaces
{
    public interface IWordsToSizesConverter
    {
        Size Size { get; set; }
        int MaxHeight { get; set; }
        int MaxWidth { get; set; }
        Result<IEnumerable<(string, Size)>> GetSizesOf(Dictionary<string, int> dictionary);
    }
}