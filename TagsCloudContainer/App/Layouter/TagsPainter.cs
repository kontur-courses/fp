using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.App.Layouter
{
    public class TagsPainter : ITagsPainter
    {
        private readonly Palette palette;

        public TagsPainter(Palette palette)
        {
            this.palette = palette;
        }

        public Result<None> Paint(IEnumerable<TagInfo> tags, Size imageSize, Graphics graphics)
        {
            return Result.OfAction(() =>
                   graphics.FillRectangle(new SolidBrush(palette.BackgroundColor), 0, 0, imageSize.Width, imageSize.Height))
               .Then(_ => DrawTags(tags, imageSize, graphics));
        }

        private Result<None> DrawTags(IEnumerable<TagInfo> tags, Size imageSize, Graphics graphics)
        {
            var i = 0;
            foreach (var tag in tags)
            {
                var result = tag.CheckIsRectangleInsideArea(imageSize)
                    .Then(tagInfo => graphics.DrawString(tag.TagText, tag.TagFont,
                        new SolidBrush(i % 2 == 0 ? palette.PrimaryColor : palette.SecondaryColor), tag.TagRect));
                if (!result.IsSuccess)
                    return Result.Fail<None>(result.Error);
                i++;
            }
            return Result.Ok();
        }
    }
}
