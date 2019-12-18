using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudResult;
using TagsCloudTextProcessing;
using TagsCloudVisualization.Styling;

namespace TagsCloudVisualization.Layouters
{
    public class SpiralLayouter : ICloudLayouter
    {
        protected readonly List<RectangleF> arrangedRectangles = new List<RectangleF>();
        protected readonly List<Tag> arrangedTags = new List<Tag>();
        protected readonly IEnumerator<PointF> spiralEnumerator;

        public SpiralLayouter(Spiral spiral)
        {
            if (spiral == null)
                throw new ArgumentException("Tag Cloud spiral can't be null.");

            spiralEnumerator = spiral.GetEnumerator();
        }

        public Result<RectangleF> PutNextRectangle(SizeF rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                return Result.Fail<RectangleF>("Tag Cloud tag size parameters should be positive.");
            var tempRect = new RectangleF(spiralEnumerator.Current, rectangleSize);
            while (arrangedRectangles.Any(r => r.IntersectsWith(tempRect)) && spiralEnumerator.MoveNext())
                tempRect.Location = spiralEnumerator.Current;
            arrangedRectangles.Add(tempRect);
            return Result.Ok(tempRect);
        }

        public Result<Tag> PutNextTag(Token token, SizeF tokenSize)
        {
            if (tokenSize.Height <= 0 || tokenSize.Width <= 0)
                return Result.Fail<Tag>("Tag Cloud tag size parameters should be positive.");
            var tempTag = new Tag(token.Word, token.Count, tokenSize, spiralEnumerator.Current);
            while (arrangedTags.Any(r => r.Area.IntersectsWith(tempTag.Area)) && spiralEnumerator.MoveNext())
                tempTag.Location = spiralEnumerator.Current;
            arrangedTags.Add(tempTag);
            return Result.Ok(tempTag);
        }
        
        public Result<List<Tag>> GenerateTagsSequence(Style style, IEnumerable<Token> tokens)
        {
            var resultList = new List<Tag>();
            foreach (var token in tokens)
            {
                var tagResult = style.TagSizeCalculator.GetScaleFactor(token.Count, style.FontProperties.MinSize)
                    .Then(scaleFactor => style.TagSizeCalculator.GetTagSize(style.FontProperties, scaleFactor, token))
                    .Then(tagSize => PutNextTag(token, tagSize));
                if(!tagResult.IsSuccess)
                    return Result.Fail<List<Tag>>(tagResult.Error);
                resultList.Add(tagResult.GetValueOrThrow());
            }
            return Result.Ok(resultList);
        }
    }
}