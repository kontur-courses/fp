using CircularCloudLayouter.Domain;

namespace TagCloudCore.Interfaces;

public interface IWordSizeCalculator
{
    ImmutableSize GetSizeFor(string word, int fontSize);
}