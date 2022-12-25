using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.App.Layouter
{
    public class TagInfo
    {
        public string TagText { get; }
        public Font TagFont { get; }
        public Rectangle TagRect { get; }

        public TagInfo(string tagText, Font tagFont, Rectangle tagRect)
        {
            TagText = tagText;
            TagFont = tagFont;
            TagRect = tagRect;
        }

        public Result<TagInfo> CheckIsRectangleInsideArea(Size imageSize)
        {
            return (TagRect.Left >= 0 && TagRect.Right <= imageSize.Width && TagRect.Top >= 0 && TagRect.Bottom <= imageSize.Height)
                ? Result.Ok(this)
                : Result.Fail<TagInfo>("Облако тегов не влезло на изображение заданного размера");
        }
    }
}