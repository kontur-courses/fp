using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class MultiDrawer
{
    private readonly IEnumerable<IDrawerFactory> drawerFactories;
    private readonly IGraphicsProvider graphicsProvider;
    private readonly IEnumerable<ILayouterAlgorithmFactory> layouterAlgorithmFactories;
    private readonly Settings settings;

    public MultiDrawer(
        Settings settings,
        IEnumerable<IDrawerFactory> drawerFactories,
        IEnumerable<ILayouterAlgorithmFactory> layouterAlgorithmFactories,
        IGraphicsProvider graphicsProvider)
    {
        this.settings = settings;
        this.drawerFactories = drawerFactories;
        this.layouterAlgorithmFactories = layouterAlgorithmFactories;
        this.graphicsProvider = graphicsProvider;
    }

    public Result Draw(IEnumerable<CloudWord> cloudWords)
    {
        return Result.Combine(GetDrawers()
            .Select(tuple => DrawCloudUsingCombination(cloudWords, tuple.drawerProvider, tuple.algorithmProvider)));
    }

    private Result DrawCloudUsingCombination(
        IEnumerable<CloudWord> cloudWords,
        IDrawerProvider drawerProvider,
        ILayouterAlgorithmProvider algorithmProvider)
    {
        return graphicsProvider.Create()
            .Bind(graphics => drawerProvider.Provide(algorithmProvider, graphics))
            .Bind(drawer => drawer.DrawCloud(cloudWords))
            .Bind(() => graphicsProvider.Commit());
    }

    private IEnumerable<(ILayouterAlgorithmProvider algorithmProvider, IDrawerProvider drawerProvider)> GetDrawers()
    {
        var (algorithmProviders, drawerProviders) = GetProviders();

        return algorithmProviders
            .SelectMany(_ => drawerProviders,
                (algorithmProvider, drawerProvider) => (algorithmProvider, drawerProvider));
    }

    private (List<ILayouterAlgorithmProvider>, List<IDrawerProvider>) GetProviders()
    {
        var algorithmProviders = layouterAlgorithmFactories
            .SelectMany(_ => settings.LayouterAlgorithmSettings,
                (factory, algorithmSettings) => factory.Build(algorithmSettings))
            .Where(result => result.IsSuccess)
            .Select(result => result.Value)
            .ToList();

        var drawerProviders = drawerFactories
            .SelectMany(_ => settings.DrawerSettings,
                (factory, drawerSettings) => factory.Build(drawerSettings))
            .Where(result => result.IsSuccess)
            .Select(result => result.Value)
            .ToList();
        return (algorithmProviders, drawerProviders);
    }
}