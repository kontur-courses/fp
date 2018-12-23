using CloodLayouter.App;
using CommandLine;
using ResultOf;

namespace CloodLayouter.Infrastructer
{
    public interface IImageSaver
    {
        Result<string> Save(ParserResult<Options> result);
    }
}