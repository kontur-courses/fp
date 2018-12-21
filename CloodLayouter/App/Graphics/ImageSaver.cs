using CloodLayouter.Infrastructer;
using CommandLine;

namespace CloodLayouter.App
{
    public class ImageSaver : IImageSaver
    {
        private readonly IDrawer drawer;
        //TODO  add SaveSettings(file format .png, .bpm, e.t.c.)


        public ImageSaver(IDrawer drawer)
        {
            this.drawer = drawer;
        }


        public void Save(ParserResult<Options> result)
        {
            result.WithParsed(opt => drawer.Draw().Save(opt.OutputFile));
        }
    }
}