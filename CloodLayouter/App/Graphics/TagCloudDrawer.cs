using System;
using System.Collections.Generic;
using System.Drawing;
using CloodLayouter.Infrastructer;
using ResultOf;

namespace CloodLayouter.App
{
    public class TagCloudDrawer : IDrawer
    {
        private readonly ICloudLayouter cloudLayouter;
        private readonly ImageSettings imageSettings;
        private readonly IProvider<IEnumerable<Result<Tag>>> tagProvider;

        public TagCloudDrawer(ICloudLayouter cloudLayouter,
            IProvider<IEnumerable<Result<Tag>>> tagProvider, ImageSettings imageSettings)
        {
            this.cloudLayouter = cloudLayouter;
            this.imageSettings = imageSettings;
            this.tagProvider = tagProvider;
        }

        public Result<Bitmap> Draw()
        {
            var bitmapResult = Result.Of(() => new Bitmap(imageSettings.Width, imageSettings.Height));
            if (!bitmapResult.IsSuccess)
                return bitmapResult;
            
            using (var grapghic = Graphics.FromImage(bitmapResult.GetValueOrThrow()))
            {
                foreach (var tag in tagProvider.Get())
                {
                    if (!tag.IsSuccess)
                    {
                        Console.WriteLine(tag.Error);
                        continue;
                    }
                    var rect = cloudLayouter.PutNextRectangle(tag.GetValueOrThrow().Size);
                    if (!rect.IsSuccess)
                    {
                        Console.WriteLine(rect.Error);
                        continue;
                    }
                    grapghic.DrawString(tag.GetValueOrThrow().Word, tag.GetValueOrThrow().Font,
                        new SolidBrush(Color.Blue), rect.GetValueOrThrow()); //HARD DEOENDENCY
                }
            }

            return bitmapResult;
        }
    }
}