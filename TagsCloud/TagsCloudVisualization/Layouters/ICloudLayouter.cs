using System.Collections.Generic;
using System.Drawing;
using TagCloudResult;
using TagsCloudTextProcessing;
using TagsCloudVisualization.Styling;

namespace TagsCloudVisualization.Layouters
{
    public interface ICloudLayouter
    {
        Result<RectangleF> PutNextRectangle(SizeF rectangleSize);

        Result<Tag> PutNextTag(Token token, SizeF tokenSize);

        Result<List<Tag>> GenerateTagsSequence(Style style, IEnumerable<Token> tokens);
    }
}