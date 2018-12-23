using System;
using System.Diagnostics.Contracts;
using CloodLayouter.Infrastructer;
using CommandLine;
using ResultOf;

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


        public Result<string> Save(ParserResult<Options> result)
        {
            var drawRes = drawer.Draw();
            if(drawRes.IsSuccess)
                return Result.Of<string>(() =>
                {
                    result.WithParsed(opt => drawer.Draw().GetValueOrThrow().Save(opt.OutputFile));
                    return "image saved";
                });
            return Result.Fail<string>("Can't save image becous -> " + drawRes.Error);
        }
    }
}