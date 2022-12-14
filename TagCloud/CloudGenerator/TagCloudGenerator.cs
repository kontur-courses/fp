using System.Drawing;
using TagCloud.ImageGenerator;
using TagCloud.Infrastructure;
using TagCloud.Parser;
using TagCloud.WordSizingAlgorithm;

namespace TagCloud.CloudGenerator;

public class TagCloudGenerator : ICloudGenerator
{
    private readonly ITagParser parser;
    private readonly IWordSizingAlgorithm wordSizingAlgorithm;
    private readonly IImageGenerator imageGenerator;

    public TagCloudGenerator(ITagParser parser, IWordSizingAlgorithm wordSizingAlgorithm, 
        IImageGenerator imageGenerator)
    {
        this.parser = parser;
        this.wordSizingAlgorithm = wordSizingAlgorithm;
        this.imageGenerator = imageGenerator;
    }

    public Result<Image> GenerateCloud(string filepath)
    {
        var tagMap = parser.Parse(filepath);

        if (!tagMap.IsSuccess)
            return new Result<Image>(tagMap.Error);

        var measuredTags = wordSizingAlgorithm.GetTagSizes(tagMap);
        return imageGenerator.GenerateImage(measuredTags);
    }
}