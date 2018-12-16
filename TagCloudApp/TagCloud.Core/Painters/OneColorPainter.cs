using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Util;

namespace TagCloud.Core.Painters
{
    public class OneColorPainter : IPainter
    {
        private readonly IPaintingSettings settings;

        public OneColorPainter(IPaintingSettings settings)
        {
            this.settings = settings;
        }

        public Result<None> PaintTags(IEnumerable<Tag> tags)
        {
            if (tags == null)
                return Result.Fail<None>("Tags can't be null");
            if (!settings.TagBrushResult.IsSuccess)
                return Result.Fail<None>($"Incorrect brush given: {settings.TagBrushResult.Error}");

            var tagsList = tags.ToList();
            tagsList.ApplyForeach(tag => tag.Brush = settings.TagBrushResult.Value);
            return Result.Ok();
        }

        public Result<None> SetBackgroundColorFor(Graphics graphics)
        {
            if (graphics == null)
                return Result.Fail<None>("Graphics can't be null");
            if (!settings.BackgroundColorResult.IsSuccess)
                return Result.Fail<None>($"Incorrect background color given: {settings.BackgroundColorResult.Error}");

            graphics.Clear(settings.BackgroundColorResult.Value);
            return Result.Ok();
        }
    }
}