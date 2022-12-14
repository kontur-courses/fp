using CircularCloudLayouter;

namespace TagCloudCore.Interfaces.Providers;

public interface ITagCloudLayouterProvider
{
    public ITagCloudLayouter CreateLayouter();
}