using System.Drawing;
using TagCloud.ResultImplementation;

namespace TagCloud.CloudLayouter.Implementation;

public abstract class CloudLayouter<T> : ICloudLayouter<T>
{
    public List<T> Figures { get; }

    protected CloudLayouter()
    {
        Figures = new List<T>();
    }

    public abstract Result<T> PutNextRectangle(Size rectangleSize);
}