using ResultOf;

namespace TagCloud
{
    public interface IApplication
    {
        Result<None> Run(string input, string output, ITextParcer textParser, ICloudLayouter cloud,
            Visualizer visualizer);
    }
}