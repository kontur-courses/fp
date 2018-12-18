using TagCloud.IntermediateClasses;
using TagCloud.Layouter;
using TagCloud.Result;

namespace TagCloud.Interfaces
{
    public interface ISizeScheme
    {
        Result<Size> GetSize(FrequentedWord word);
    }
}