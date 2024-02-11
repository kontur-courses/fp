using System.Drawing;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.DrawRectangle;

public interface IDraw
{
    public Result<Bitmap> CreateImage(List<Word> words);
}
