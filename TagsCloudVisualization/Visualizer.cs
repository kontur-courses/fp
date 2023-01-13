using TagsCloudVisualization.Drawer;
using TagsCloudVisualization.Preprocessors;
using TagsCloudVisualization.TagFactory;
using TagsCloudVisualization.TextProviders;
using TagsCloudVisualization.ToTagConverter;

namespace TagsCloudVisualization;

public class Visualizer
{
    private readonly ITextProvider textProvider;
    private readonly IPreprocessor preprocessor;
    private readonly IToTagConverter tagConverter;
    private readonly ITagFactory tagFactory;
    private readonly IDrawer drawer;

    public Visualizer(ITextProvider textProvider,
        IPreprocessor preprocessor,
        IToTagConverter tagConverter,
        ITagFactory tagFactory,
        IDrawer drawer)
    {
        this.textProvider = textProvider;
        this.preprocessor = preprocessor;
        this.tagConverter = tagConverter;
        this.tagFactory = tagFactory;
        this.drawer = drawer;
    }

    public Result<None> Visualize(string path, int tagCount)
    {
        return tagCount.AsResult()
            .Validate(c => c > 0, $"{nameof(tagCount)} should be positive")
            .Then(textProvider.GetText)
            .Then(preprocessor.Process)
            .Then(tagConverter.Convert)
            .Then(tags => tags.Take(tagCount).Select(x => tagFactory.Create(x).Value).ToArray())
            .Then(tags => drawer.Draw(tags, path));
    }
}