using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.App.Layouter
{
    public class TagsPainter : ITagsPainter
    {
        private readonly IImageHolder imageHolder;
        private readonly Palette palette;

        public TagsPainter(IImageHolder imageHolder, Palette palette)
        {
            this.imageHolder = imageHolder;
            this.palette = palette;
        }

        public Result<None> Paint(IEnumerable<TagInfo> tags)
        {
            var imageSize = imageHolder.GetImageSize();
            if (!imageSize.IsSuccess)
                return Result.Fail<None>(imageSize.Error);
            var graphics = imageHolder.StartDrawing();
            if (!graphics.IsSuccess)
                return Result.Fail<None>(graphics.Error);
            graphics.Value.FillRectangle(new SolidBrush(palette.BackgroundColor), 0, 0, imageSize.Value.Width, imageSize.Value.Height);
            var i = 0;
            foreach (var tag in tags)
            {
                var result = tag.CheckIsRectangleInsideArea(imageSize.Value)
                    .Then(tagInfo => graphics.Value.DrawString(tag.TagText, tag.TagFont, 
                        new SolidBrush(i % 2 == 0 ? palette.PrimaryColor : palette.SecondaryColor), tag.TagRect));
                if (!result.IsSuccess)
                    return Result.Fail<None>(result.Error);
                i++;
            }
            imageHolder.UpdateUi();
            return Result.Ok();
        }
    }
}
