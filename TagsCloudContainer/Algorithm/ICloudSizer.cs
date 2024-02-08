using TagsCloudContainer.Infrastucture;
using TagsCloudContainer.Infrastucture.Settings;

namespace TagsCloudContainer.Algorithm
{
    public interface ICloudSizer
    {
        (Font, SizeF) GetCloudSize(Dictionary<string, int> wordFrequencies, string word);
    }
}