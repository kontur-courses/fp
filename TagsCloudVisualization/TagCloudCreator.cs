using Results;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.ImageSavers;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualization;

public class TagCloudCreator
{
    private readonly ITagLayouter tagLayouter;
    private readonly IVisualizer visualizer;
    private readonly IImageSaver imageSaver;

    public TagCloudCreator(ITagLayouter tagLayouter, IVisualizer visualizer, IImageSaver imageSaver)
    {
        this.tagLayouter = tagLayouter;
        this.visualizer = visualizer;
        this.imageSaver = imageSaver;
    }

    public Result<None> CreateAndSaveImage()
    {
        var tags = tagLayouter.GetTags();
        var image = visualizer.Vizualize(tags);
        if (!image.IsSuccess)
            return Result.Fail<None>(image.Error);
        var result = imageSaver.SaveImage(image.Value);
        if (!result.IsSuccess)
            return Result.Fail<None>(result.Error);
        return Result.Ok();
    }
}