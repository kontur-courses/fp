namespace TagsCloudContainer.Interfaces;

public class Settings
{
    public IReadOnlyCollection<DrawerSettings> DrawerSettings { get; init; } = new List<DrawerSettings>();

    public IReadOnlyCollection<LayouterAlgorithmSettings> LayouterAlgorithmSettings { get; init; } =
        new List<LayouterAlgorithmSettings>();

    public GraphicsProviderSettings GraphicsProviderSettings { get; init; } = null!;
}