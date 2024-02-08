using TagsCloudContainer.Infrastucture;

namespace TagsCloudContainer.Algorithm
{
    public interface ICloudLayouter
    {
        Result<List<TextRectangle>> GetRectangles(Dictionary<string, int> wordFrequencies);
    }
}