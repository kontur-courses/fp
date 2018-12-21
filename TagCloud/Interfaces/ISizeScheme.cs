using TagCloud.IntermediateClasses;
using TagCloud.Layouter;

namespace TagCloud.Interfaces
{
    public interface ISizeScheme
    {
        Result<Size> GetSize(FrequentedWord word);
    }
}