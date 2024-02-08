using System.Drawing;
using TagsCloud.App.Infrastructure;
using TagsCloud.App.Settings;
using TagsCloud.CloudLayouter;
using TagsCloud.CloudVisualizer;
using TagsCloud.WordAnalyzer;

namespace TagsCloud.App;

public class TagCloudPainter
{
    private readonly FileReader fileReader;

    private readonly IImageHolder imageHolder;
    private readonly TagSettings tagSettings;
    private readonly WordAnalyzer.WordAnalyzer wordAnalyzer;

    public TagCloudPainter(IImageHolder imageHolder, TagSettings tagSettings, WordAnalyzer.WordAnalyzer wordAnalyzer,
        FileReader reader)
    {
        this.imageHolder = imageHolder;
        this.wordAnalyzer = wordAnalyzer;
        fileReader = reader;
        this.tagSettings = tagSettings;
    }

    public Result<None> Paint(string filePath, ISpiral spiral)
    {
        var result = fileReader.GetWords(filePath)
            .Then(words => wordAnalyzer.GetFrequencyList(words))
            .Then(frequencyList => imageHolder.GetImageSize()
                .Then(sizeImage => DrawTagCloud(spiral, frequencyList, sizeImage)));
        if (result.IsSuccess)
            imageHolder.UpdateUi();
        return result;
    }

    private Result<None> DrawTagCloud(ISpiral spiral, IEnumerable<WordInfo> frequencyList, Size sizeImage)
    {
        var painter = new TagCloudVisualizer(tagSettings, sizeImage);
        var cloudLayouter =
            new CloudLayouter.CloudLayouter(spiral, new Point(sizeImage.Width / 2, sizeImage.Height / 2));
        var background = new SolidBrush(Color.Black);
        using var graphic = imageHolder.StartDrawing().GetValueOrThrow();
        graphic.FillRectangle(background, new Rectangle(0, 0, sizeImage.Width, sizeImage.Height));
        return painter.DrawTags(frequencyList, graphic, cloudLayouter);
    }
}