namespace TagsCloudVisualization.Drawer;

public interface IDrawer
{
    Result<None> Draw(IReadOnlyCollection<IDrawImage> drawImages,string filepath);
}