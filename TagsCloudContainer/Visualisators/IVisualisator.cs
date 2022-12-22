using System.Drawing;
using TagsCloudContainer.WorkWithWords;

namespace TagsCloudContainer.Visualisators
{
    public interface IVisualisator
    {
        public Result<Bitmap> Paint(List<Word> words);
    }
}