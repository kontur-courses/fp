using Results;
using System.Drawing;
using TagsCloudVisualization.Settings;
using TagsCloudVisualization.Tags;
using TagsCloudVisualization.TextHandlers;

namespace TagsCloudVisualization.CloudLayouters;

public class TagsLayouter : ITagLayouter
{
    private readonly ICloudLayouter cloudLayouter;
    private readonly ITextHandler textHandler;
    private readonly Graphics graphics;
    private readonly TagsLayouterSettings settings;

    public TagsLayouter(ICloudLayouter cloudLayouter, ITextHandler textHandler, TagsLayouterSettings settings) 
    {
        this.cloudLayouter = cloudLayouter;
        this.textHandler = textHandler;
        this.settings = settings;
        graphics = Graphics.FromHwnd(IntPtr.Zero);
    }

    public Result<IList<Tag>> GetTags()
    {
        var handledText = textHandler.HandleText();
        if (!handledText.IsSuccess)
            return Result.Fail<IList<Tag>>(handledText.Error);
        var checkSettings = settings.Check();
        if (!checkSettings.IsSuccess)
            return Result.Fail<IList<Tag>>(checkSettings.Error);

        var sortedWords = handledText.Value.OrderByDescending(p => p.Value);
        var maxWordCount = sortedWords.First().Value;
        var minWordCount = sortedWords.Last().Value;
        IList<Tag> result = new List<Tag>();
        var fontFamily = new FontFamily(settings.FontFamily);

        foreach (var wordWithCount in sortedWords)
        {
            var fontSize = GetFontSize(minWordCount, maxWordCount, wordWithCount.Value);
            var rectangle = cloudLayouter.PutNextRectangle(GetWordSize(wordWithCount.Key, fontSize, fontFamily));
            if (!rectangle.IsSuccess)
                return Result.Fail<IList<Tag>>(rectangle.Error);
            result.Add(new Tag(wordWithCount.Key, 
                fontSize,
                rectangle.Value,
                fontFamily));
        }
        return Result.Ok(result);
    }

    private int GetFontSize(int minWordCount, int maxWordCount, int wordCount)
    {
        return maxWordCount > minWordCount ? settings.MinSize 
            + (settings.MaxSize - settings.MinSize) 
            * (wordCount - minWordCount) 
            / (maxWordCount - minWordCount) : settings.MaxSize;
    }

    private Size GetWordSize(string content, int fontSize, FontFamily fontFamily)
    {
        var sizeF = graphics.MeasureString(content, new Font(fontFamily, fontSize));
        return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
    }
}