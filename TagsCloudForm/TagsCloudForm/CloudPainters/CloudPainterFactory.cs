using System;
using System.Drawing;
using CircularCloudLayouter;
using TagsCloudForm.Common;

namespace TagsCloudForm.CloudPainters
{
    public class CloudPainterFactory : IPainterFactory
    {
        private readonly IImageHolder imageHolder;
        private readonly IPalette palette;
        private readonly Func<Point, ICircularCloudLayouter> makeLayouter;
        private readonly CircularCloudLayouterSettings.ICircularCloudLayouterSettings settings;
        public CloudPainterFactory(IImageHolder imageHolder,
            IPalette palette,
            CircularCloudLayouterSettings.ICircularCloudLayouterSettings settings,
            Func<Point, ICircularCloudLayouter> makeLayouter)
        {
            
            this.imageHolder = imageHolder;
            this.palette = palette;
            this.makeLayouter = makeLayouter;
            this.settings = settings;
        }

        public Result<ICloudPainter> Create()
        {
            return Result
                .Of(() => makeLayouter(new Point(settings.CenterX, settings.CenterY)))
                .ThenAct(x=>x.SetCompression(settings.XCompression, settings.YCompression))
                .Then(x=> new CloudPainter(imageHolder, settings, palette, x))
                .Then(x=>(ICloudPainter)x);
        }
    }
}
