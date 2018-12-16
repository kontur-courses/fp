using System;
using System.Collections;
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
            var tagsList = tags.ToList();
            tagsList.ApplyForeach(tag => tag.Brush = settings.TagBrush);
            return Result.Ok();
        }

        public Result<None> SetBackgroundColorFor(Graphics graphics)
        {
            if (graphics == null)
                return Result.Fail<None>("Graphics can't be null");

            graphics.Clear(settings.BackgroundColor);
            return Result.Ok();
        }
    }
}