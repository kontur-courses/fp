using System.Drawing;
using ResultOf;
using TagCloud.Common.Drawing;
using TagCloud.Common.Options;
using TagCloud.Common.TagsConverter;
using TagCloud.Common.TextFilter;

namespace TagCloud.Common;

public class TagCloudCreator
{
    private ICloudDrawer drawer;
    private ITagsConverter converter;
    private ITextFilter filter;

    public TagCloudCreator(ICloudDrawer drawer, ITagsConverter converter, ITextFilter filter)
    {
        this.drawer = drawer;
        this.converter = converter;
        this.filter = filter;
    }

    public Result<Bitmap> CreateCloud(WordsOptions wordsOptions)
    {
        if (!File.Exists(wordsOptions.PathToTextFile))
        {
            return Result.Fail<Bitmap>("Path to text file was incorrect");
        }

        var lines = File.ReadAllLines(wordsOptions.PathToTextFile);
        var cloudImage = filter.FilterAllWords(lines, wordsOptions.BoringWordsThreshold)
            .Then(w => converter.ConvertToTags(w, wordsOptions.MinFontSize))
            .Then(tags => drawer.DrawCloud(tags));
        return cloudImage;
    }
}