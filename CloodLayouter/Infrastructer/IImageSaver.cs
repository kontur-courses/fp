using CloodLayouter.App;
using CommandLine;

namespace CloodLayouter.Infrastructer
{
    public interface IImageSaver
    {
        void Save(ParserResult<Options> result);
    }
}