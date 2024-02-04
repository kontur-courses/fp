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

    public IEnumerable<Result<Tag>> GetTags()
    {
        var handledText = textHandler.HandleText();
        if (!handledText.IsSuccess)
        {
            yield return Result.Fail<Tag>(handledText.Error);
            yield break;
        }
        if (!settings.FontFamily.IsSuccess)
        {
            yield return Result.Fail<Tag>(settings.FontFamily.Error);
            yield break;
        }
        if (!settings.MinSize.IsSuccess)
        {
            yield return Result.Fail<Tag>(settings.MinSize.Error);
            yield break;
        }
        if (!settings.MaxSize.IsSuccess)
        {
            yield return Result.Fail<Tag>(settings.MaxSize.Error);
            yield break;
        }
        var sortedWords = handledText.Value.OrderByDescending(p => p.Value);
        var maxWordCount = sortedWords.First().Value;
        var minWordCount = sortedWords.Last().Value;

        foreach (var wordWithCount in sortedWords)
        {
            var fontSize = GetFontSize(minWordCount, maxWordCount, wordWithCount.Value);
            var rectangle = cloudLayouter.PutNextRectangle(GetWordSize(wordWithCount.Key, fontSize));
            if (!rectangle.IsSuccess)
            {
                yield return Result.Fail<Tag>(rectangle.Error);
                yield break;
            }
            yield return new Tag(wordWithCount.Key, 
                fontSize,
                rectangle.GetValueOrThrow(),
                settings.FontFamily.Value);
        }
    }

    private int GetFontSize(int minWordCount, int maxWordCount, int wordCount)
    {
        return maxWordCount > minWordCount ? settings.MinSize.Value 
            + (settings.MaxSize.Value - settings.MinSize.Value) 
            * (wordCount - minWordCount) 
            / (maxWordCount - minWordCount) : settings.MaxSize.Value;
    }

    private Size GetWordSize(string content, int fontSize)
    {
        var sizeF = graphics.MeasureString(content, new Font(settings.FontFamily.Value, fontSize));
        return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
    }
}